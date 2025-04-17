using Basket.API.Context;
using Basket.API.DTOs.Product;
using Basket.API.DTOs.Requests;
using Basket.API.Entities;
using Basket.API.EventStore;
using Basket.API.Outbox;
using Basket.API.Services.LoginService;
using Basket.API.Services.ProductService;
using Caching.Redis.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BasketController : ControllerBase
    {
        private readonly IRedisCache _cache;
        private readonly TimeSpan _cacheExpiry = TimeSpan.FromDays(2);
        private readonly ILoginService _loginService;
        private readonly IProductService _productService;
        private readonly IEventStoreHandler _eventStoreHandler;
        private readonly BasketDbContext _dbContext;

        public BasketController(IRedisCache cache, ILoginService loginService, IProductService productService, IEventStoreHandler eventStoreHandler,BasketDbContext dbContext)
        {
            _cache = cache;
            _loginService = loginService;
            _productService = productService;
            _eventStoreHandler = eventStoreHandler;
            _dbContext = dbContext;
        }

        [HttpPost("AddItem")]
        public async Task<IActionResult> AddItemToBasket(AddItemToBasketRequestDTO addItemToBasketRequestDTO)
        {
            var userId = _loginService.UserId;
            string cacheKey = $"Basket-{userId}";
            var cachedResponse = await _cache.GetAsync<string>(cacheKey);
            ProductDTO product = await _productService.GetProductById(addItemToBasketRequestDTO.ProductId);
            Basket.API.Entities.Basket basket = new();
            if (product == null)
            {
                return NotFound("Product not found");
            }
            if (cachedResponse != null)
            {
                basket = Newtonsoft.Json.JsonConvert.DeserializeObject<Basket.API.Entities.Basket>(cachedResponse);
            }
            basket.AddItem(addItemToBasketRequestDTO.ProductId, 1, product.Price, product.Name);
            await _cache.AddAsync(
                cacheKey,
                Newtonsoft.Json.JsonConvert.SerializeObject(basket),
                _cacheExpiry
            );
            _eventStoreHandler.AppendToStreamAsync(cacheKey, "ItemAdded", basket);
            return Ok("Item added to basket");
        }

        [HttpPost("RemoveItem")]
        public async Task<IActionResult> RemoveItemFromBasket(RemoveItemFromBasketRequestDTO removeItemFromBasketRequestDTO)
        {
            var userId = _loginService.UserId;
            string cacheKey = $"Basket-{userId}";
            var cachedResponse = await _cache.GetAsync<string>(cacheKey);
            if (cachedResponse == null)
            {
                return NotFound("Basket not found");
            }
            var basket = Newtonsoft.Json.JsonConvert.DeserializeObject<Basket.API.Entities.Basket>(cachedResponse);
            basket.RemoveItem(removeItemFromBasketRequestDTO.ProductId);
            await _cache.AddAsync(
                cacheKey,
                Newtonsoft.Json.JsonConvert.SerializeObject(basket),
                _cacheExpiry
            );
            _eventStoreHandler.AppendToStreamAsync(cacheKey, "ItemRemoved", basket);
            return Ok("Item removed from basket");
        }

        [HttpPost("DeleteBasket")]
        public async Task<IActionResult> DeleteBasket()
        {
            var userId = _loginService.UserId;
            string cacheKey = $"Basket-{userId}";
            var cachedResponse = await _cache.GetAsync<string>(cacheKey);
            if (cachedResponse == null)
            {
                return NotFound("Basket not found");
            }
            _cache.RemoveAsync(cacheKey);
            _eventStoreHandler.AppendToStreamAsync(cacheKey, "BasketDeleted", cachedResponse);
            return Ok("Basket deleted");
        }

        [HttpGet("GetBasket")]
        public async Task<IActionResult> GetBasket()
        {
            var userId = _loginService.UserId;
            string cacheKey = $"Basket-{userId}";
            var cachedResponse = await _cache.GetAsync<string>(cacheKey);
            if (cachedResponse == null)
            {
                return NotFound("Basket not found");
            }
            var basket = Newtonsoft.Json.JsonConvert.DeserializeObject<Basket.API.Entities.Basket>(cachedResponse);
            return Ok(basket);
        }

        [HttpPost("Checkout")]
        public async Task<IActionResult> Checkout(CheckoutDTO checkoutDTO)
        {
            var userId = _loginService.UserId;
            string cacheKey = $"Basket-{userId}";
            var cachedResponse = await _cache.GetAsync<string>(cacheKey);
            if (cachedResponse == null)
            {
                return NotFound("Basket not found");
            }
            var basket = Newtonsoft.Json.JsonConvert.DeserializeObject<Basket.API.Entities.Basket>(cachedResponse);
            Checkout checkout = new()
            {
                UserId = userId,
                TotalPrice = basket.BasketAmount,
                BasketItems = basket.BasketItems.ToArray(),
                UserInformation = new()
                {
                    FirstName = checkoutDTO.FirstName,
                    LastName = checkoutDTO.LastName,
                    Email = checkoutDTO.Email,
                    PhoneNumber = checkoutDTO.PhoneNumber,
                    Address = checkoutDTO.Address
                },
                CardInformation = new()
                {
                    CardNumber = checkoutDTO.CardNumber,
                    CardHolderName = checkoutDTO.CardHolderName,
                    ExpirationDate = checkoutDTO.ExpirationDate,
                    CVC = checkoutDTO.CVC
                }
            };

            _dbContext.OutboxMessage.Add(new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Payload = Newtonsoft.Json.JsonConvert.SerializeObject(checkout),
                Processed = false
            });
            await _dbContext.SaveChangesAsync();

            _eventStoreHandler.AppendToStreamAsync(cacheKey, "BasketCheckout", checkout);

            _cache.RemoveAsync(cacheKey);

            return Ok("Checkout successful");
        }
    }
}

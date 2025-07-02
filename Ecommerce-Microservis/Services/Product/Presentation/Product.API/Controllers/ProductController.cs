using AutoMapper;
using Caching.Redis.Interface;
using Ecommerce.Base.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Product.Application.Features.Queries.GetAllProduct;
using Product.Application.Features.Queries.GetProductById;
using Product.Application.Requests.GetAllProduct;
using Product.Application.Requests.GetProductById;
using Product.Application.Responses.GetAllProduct;
using Product.Application.Responses.GetProductById;

namespace Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IRedisCache _cache;
        private readonly TimeSpan _cacheExpiry = TimeSpan.FromMinutes(30);

        public ProductController(IMediator mediator, IMapper mapper, IRedisCache cache)
        {
            _mediator = mediator;
            _mapper = mapper;
            _cache = cache;
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetProductById([FromRoute] GetProductByIdRequest getProductByIyRequest)
        {
            string cacheKey = $"Product-{getProductByIyRequest.Id}";
            var cachedResponse = await _cache.GetAsync<string>(cacheKey);
            if (cachedResponse != null)
            {
                return Ok(JsonConvert.DeserializeObject<GetProductByIdResponse>(cachedResponse));
            }

            var query = _mapper.Map<GetProductByIdRequest, GetProductByIdQuery>(getProductByIyRequest);
            GetProductByIdResponse response = await _mediator.Send(query);
            _ = _cache.AddAsync(
                cacheKey,
                JsonConvert.SerializeObject(response),
                _cacheExpiry
            );
            return Ok(response);
        }

        
        [HttpGet("list")]
        public async Task<IActionResult> GetAllProducts([FromQuery] GetAllProductRequest request)
        {
            string cacheKey = $"ProductList_Page{request.Page}_Size{request.PageSize}";
            var cachedResponse = await _cache.GetAsync<string>(cacheKey);
            if (cachedResponse != null)
            {
                return Ok(JsonConvert.DeserializeObject<GetAllProductResponse>(cachedResponse));
            }

            var query = _mapper.Map<GetAllProductRequest, GetAllProductsQuery>(request);
            GetAllProductResponse response = await _mediator.Send(query);
            _ = _cache.AddAsync(
                cacheKey,
                JsonConvert.SerializeObject(response),
                _cacheExpiry
            );
            return Ok(response);
        }
    }
}

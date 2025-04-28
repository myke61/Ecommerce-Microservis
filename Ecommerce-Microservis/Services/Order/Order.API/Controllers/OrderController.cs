using AutoMapper;
using Caching.Redis.Interface;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.API.Services.LoginService;
using Order.Application.Features.Queries.GetOrders;
using Order.Application.Requests.GetOrders;
using Order.Application.Responses.GetOrders;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly IRedisCache _cache;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public OrderController(ILoginService loginService, IRedisCache cache,IMapper mapper, IMediator mediator)
        {
            _loginService = loginService;
            _cache = cache;
            _mapper = mapper;
            _mediator = mediator;
        }


        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var userId = _loginService.UserId;
            string cacheKey = $"Orders-{userId}";
            var cachedResponse = _cache.GetAsync<string>(cacheKey).Result;
            GetOrdersRequest getOrdersRequest = new()
            {
                UserId = userId
            };
            if (cachedResponse != null)
            {
                return Ok(Newtonsoft.Json.JsonConvert.DeserializeObject<GetOrdersResponse>(cachedResponse));
            }

            var query = _mapper.Map<GetOrdersRequest, GetOrdersQuery>(getOrdersRequest);
            GetOrdersResponse response = await _mediator.Send(query);
            return Ok(response);
        }
    }
}

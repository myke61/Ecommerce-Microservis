using AutoMapper;
using Azure.Core;
using Caching.Redis.Interface;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Product.Application.Features.Queries.GetAllCategories;
using Product.Application.Features.Queries.GetAllProduct;
using Product.Application.Requests.GetAllCategories;
using Product.Application.Requests.GetAllProduct;
using Product.Application.Responses.GetAllCategory;

namespace Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IRedisCache _cache;
        private readonly TimeSpan _cacheExpiry = TimeSpan.FromMinutes(30);

        public CategoryController(IRedisCache cache,IMapper mapper,IMediator mediator)
        {
            _mediator = mediator;
            _mapper = mapper;
            _cache = cache;
        }


        [HttpGet("list")]
        public async Task<IActionResult> GetCategories([FromQuery] GetAllCategoryRequest request)
        {
            string cacheKey = "CategoryList";
            var cachedResponse = _cache.GetAsync<string>(cacheKey).Result;
            if (cachedResponse != null)
            {
                return Ok(JsonConvert.DeserializeObject<GetAllCategoryResponse>(cachedResponse));
            }
            
            var query = _mapper.Map<GetAllCategoryRequest, GetAllCategoriesQuery>(request);
            GetAllCategoryResponse response = await _mediator.Send(query);
            _ = _cache.AddAsync(
                cacheKey,
                JsonConvert.SerializeObject(response),
                _cacheExpiry
            );
            return Ok(response);
        }
    }
}

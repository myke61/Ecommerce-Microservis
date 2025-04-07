using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Features.Command.CreateProduct;
using Product.Application.Features.Command.DeleteProduct;
using Product.Application.Features.Command.UpdateProduct;
using Product.Application.Features.Queries.GetAllProduct;
using Product.Application.Features.Queries.GetProductById;
using Product.Application.Requests.CreateProduct;
using Product.Application.Requests.DeleteProduct;
using Product.Application.Requests.GetAllProduct;
using Product.Application.Requests.GetProductById;
using Product.Application.Requests.UpdateProduct;
using Product.Application.Responses.DeleteProduct;
using Product.Application.Responses.GetAllProduct;
using Product.Application.Responses.GetProductById;
using Product.Application.Responses.UpdateProduct;

namespace Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public ProductController(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct(CreateProductRequest createProductRequest)
        {
            var command = mapper.Map<CreateProductRequest, CreateProductCommand>(createProductRequest);
            await mediator.Send(command);
            return Ok("Product Created!");
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetProductById([FromRoute] GetProductByIdRequest getProductByIyRequest)
        {
            var query = mapper.Map<GetProductByIdRequest, GetProductByIdQuery>(getProductByIyRequest);
            GetProductByIdResponse response = await mediator.Send(query);
            return Ok(response);
        }

        [HttpPut("update/{productId}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] Guid productId, UpdateProductRequest updateProductRequest)
        {
            updateProductRequest.Id = productId;
            //updateProductRequest.UserId = userId;
            var query = mapper.Map<UpdateProductRequest, UpdateProductCommand>(updateProductRequest);
            UpdateProductResponse response = await mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllProducts([FromQuery] GetAllProductRequest request)
        {
            var query = mapper.Map<GetAllProductRequest, GetAllProductsQuery>(request);
            GetAllProductResponse response = await mediator.Send(query);
            return Ok(response);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] DeleteProductRequest request)
        {
            var query = mapper.Map<DeleteProductRequest, DeleteProductCommand>(request);
            DeleteProductResponse response = await mediator.Send(query);
            return Ok(response);
        }
    }
}

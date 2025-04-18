using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Features.Command.CreateProduct;
using Product.Application.Features.Command.DeleteProduct;
using Product.Application.Features.Command.UpdateProduct;
using Product.Application.Requests.CreateProduct;
using Product.Application.Requests.DeleteProduct;
using Product.Application.Requests.UpdateProduct;
using Product.Application.Responses.DeleteProduct;
using Product.Application.Responses.UpdateProduct;
using System.Security.Claims;

namespace Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductAdminController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public ProductAdminController(IMediator mediator, IMapper mapper)
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

        [HttpPut("update/{productId}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] Guid productId, UpdateProductRequest updateProductRequest)
        {
            updateProductRequest.Id = productId;
            var query = mapper.Map<UpdateProductRequest, UpdateProductCommand>(updateProductRequest);
            UpdateProductResponse response = await mediator.Send(query);
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

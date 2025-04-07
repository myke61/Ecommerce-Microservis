using Ecommerce.Base.Responses;
using MediatR;
using Product.Application.Responses.CreateProduct;

namespace Product.Application.Features.Command.CreateProduct
{
    public class CreateProductCommand : IRequest<CreateProductResponse>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string ImageURL { get; set; }
        public decimal Price { get; set; }
    }
}

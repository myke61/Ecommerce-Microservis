using MediatR;
using Product.Application.Responses.UpdateProduct;

namespace Product.Application.Features.Command.UpdateProduct
{
    public class UpdateProductCommand : IRequest<UpdateProductResponse>
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Category { get; set; }
        public required string ImageURl {  get; set; }
        public decimal Price { get; set; }
    }
}

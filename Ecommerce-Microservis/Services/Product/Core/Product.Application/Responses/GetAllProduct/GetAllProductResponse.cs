using Ecommerce.Base.Responses;

namespace Product.Application.Responses.GetAllProduct
{
    public class GetAllProductResponse
    {
        public List<Domain.Entities.Product> Products { get; set; }
    }
}

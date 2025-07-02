using Ecommerce.Base.Responses;

namespace Product.Application.Responses.GetAllProduct
{
    public class GetAllProductResponse
    {
        public List<Domain.Entities.Product> Products { get; set; }
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}

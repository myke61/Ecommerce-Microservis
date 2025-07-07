using Ecommerce.Base.Responses;

namespace Product.Application.Responses.GetAllProduct
{
    public class GetAllProductResponse
    {
        public List<ProductDto> Products { get; set; }
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }

    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public BrandDto Brand { get; set; }
        public CategoryDto Category { get; set; }
        public ICollection<ProductVariantDto> Variants { get; set; }
        public ICollection<ProductImageDto> Images { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class BrandDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class ProductVariantDto
    {
        public Guid Id { get; set; }
        public string Sku { get; set; }
        public decimal Price { get; set; }
    }

    public class ProductImageDto
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
    }
}

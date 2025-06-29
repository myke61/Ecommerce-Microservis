using Ecommerce.Base.Entities;
namespace Product.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public bool IsDeleted { get; set; }
        public Guid BrandId { get; set; }
        public Brand Brand { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<ProductVariant> Variants { get; set; }
        public ICollection<ProductImage> Images { get; set; }
    }
}

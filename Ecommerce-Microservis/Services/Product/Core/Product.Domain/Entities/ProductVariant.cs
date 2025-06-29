using Ecommerce.Base.Entities;

namespace Product.Domain.Entities
{
    public class ProductVariant : BaseEntity
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public string Sku { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public ICollection<ProductVariantOption> VariantOptions { get; set; }
    }
}

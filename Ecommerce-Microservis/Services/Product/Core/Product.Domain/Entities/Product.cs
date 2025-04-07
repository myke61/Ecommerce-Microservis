using Ecommerce.Base.Entities;
namespace Product.Domain.Entities
{
    public class Product : BaseEntity
    {
        public Product(string code,string name, string category,string imageURL,decimal price)
        {
            Code = code;
            Name = name;
            Category = category;
            ImageURL = imageURL;
            Price = price;
        }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string ImageURL { get; set; }
        public decimal Price { get; set; }
        public bool IsDeleted { get; set; }
    }
}

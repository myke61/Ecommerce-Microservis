using Ecommerce.Base.Responses;
using Product.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Application.Responses.GetProductById
{
    public class GetProductByIdResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public ProductBrand Brand { get; set; }
        public ProductCategory Category { get; set; }

        public ICollection<ProductVariantMapper> Variants { get; set; }
        public ICollection<ProductImageMapper> Images { get; set; }

    }

    public class ProductBrand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LogoUrl { get; set; }
    }

    public class ProductCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
    }

    public class ProductVariantMapper
    {
        public Guid Id { get; set; }
        public string Sku { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }

    public class ProductVariantOptionMapper
    {

    }

    public class ProductImageMapper
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsMain { get; set; }
    }
}

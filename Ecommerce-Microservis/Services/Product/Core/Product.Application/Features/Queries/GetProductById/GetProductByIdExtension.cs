using Product.Application.Responses.GetProductById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Application.Features.Queries.GetProductById
{
    public static class GetProductByIdExtension
    {
        public static GetProductByIdResponse Map(this Domain.Entities.Product product)
        {
            List<ProductVariantMapper> variants = [];
            List<ProductImageMapper> images = [];
            foreach (var item in product.Variants)
            {
                var dataVariant = new ProductVariantMapper
                {
                    Id = item.Id,
                    Sku = item.Sku,
                    Price = item.Price,
                    StockQuantity = item.StockQuantity,
                };
                variants.Add(dataVariant);
            }
            foreach (var image in product.Images)
            {
                var dataImage = new ProductImageMapper
                {
                    Id = image.Id,
                    DisplayOrder = image.DisplayOrder,
                    ImageUrl = image.ImageUrl,
                    IsMain = image.IsMain,
                };
                images.Add(dataImage);
            }
            return new GetProductByIdResponse
            {
                Id = product.Id,
                Code = product.Code,
                Name = product.Name,
                Description = product.Description,
                Slug = product.Slug,
                Brand = new ProductBrand
                {
                    Id = product.Brand.Id,
                    Name = product.Brand.Name,
                    Description = product.Brand.Description,
                    LogoUrl = product.Brand.LogoUrl
                },
                Category = new ProductCategory
                {
                    Id = product.Category.Id,
                    Name = product.Category.Name,
                    Description = product.Category.Description,
                    Slug = product.Category.Slug
                },
                Variants = variants,
                Images = images
            };
        }

    }
}

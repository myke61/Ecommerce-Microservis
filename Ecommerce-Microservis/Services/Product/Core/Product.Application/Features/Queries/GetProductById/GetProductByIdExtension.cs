using Product.Application.Responses.GetProductById;
using Product.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Application.Features.Queries.GetProductById
{
    public static class GetProductByIdExtension
    {
        public static GetProductByIdResponse Map(this ProductVariant productVariant)
        {
            return new GetProductByIdResponse
            {
                Id = productVariant.Id,
                Code = productVariant.Product.Code,
                Name = productVariant.Product.Name,
                Description = productVariant.Product.Description,
                Slug = productVariant.Product.Slug,
                Price = productVariant.Price,
                Brand = new ProductBrand
                {
                    Id = productVariant.Product.Brand.Id,
                    Name = productVariant.Product.Brand.Name,
                    Description = productVariant.Product.Brand.Description,
                    LogoUrl = productVariant.Product.Brand.LogoUrl
                },
                Category = new ProductCategory
                {
                    Id = productVariant.Product.Category.Id,
                    Name = productVariant.Product.Category.Name,
                    Description = productVariant.Product.Category.Description,
                    Slug = productVariant.Product.Category.Slug
                },
                Images = [.. productVariant.Product.Images.Select(image => new ProductImageMapper
                {
                    Id = image.Id,
                    ImageUrl = image.ImageUrl,
                    DisplayOrder = image.DisplayOrder,
                    IsMain = image.IsMain
                })]
            };
        }

    }
}

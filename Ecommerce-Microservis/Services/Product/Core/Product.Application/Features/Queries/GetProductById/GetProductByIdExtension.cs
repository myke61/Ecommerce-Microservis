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
            return new GetProductByIdResponse
            {
                Id = product.Id,
                Code = product.Code,
                Name = product.Name,
                Price = product.Price,
                Category = product.Category,
                ImageURL = product.ImageURL

            };
        }

    }
}

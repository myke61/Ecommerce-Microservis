﻿using Product.Application.Responses.GetAllProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Application.Features.Queries.GetAllProduct
{
    public static class GetAllProductsExtensions
    {
        public static GetAllProductResponse Map(this List<ProductVariantDto> products)
        {
            return new GetAllProductResponse
            {
                ProductVariants = products,
            };
        }

    }
}

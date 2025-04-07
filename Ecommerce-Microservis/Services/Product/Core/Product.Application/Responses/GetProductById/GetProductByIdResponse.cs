using Ecommerce.Base.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Application.Responses.GetProductById
{
    public class GetProductByIdResponse
    {
        public string Code { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string ImageURL { get; set; }
        public decimal Price { get; set; }

    }
}

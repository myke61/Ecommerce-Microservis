using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Application.Requests.UpdateProduct
{
    public class UpdateProductRequest
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Category { get; set; }
        public required string ImageURL { get; set; }
        public decimal Price { get; set; }
    }
}

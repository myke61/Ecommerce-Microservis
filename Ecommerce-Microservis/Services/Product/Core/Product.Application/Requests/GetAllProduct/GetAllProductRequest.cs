using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Application.Requests.GetAllProduct
{
    public class GetAllProductRequest
    {
        public int Page { get; set; } = 1; 
        public int PageSize { get; set; } = 10; 
    }
}

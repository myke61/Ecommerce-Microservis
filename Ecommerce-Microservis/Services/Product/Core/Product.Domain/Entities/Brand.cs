using Ecommerce.Base.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain.Entities
{
    public class Brand : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string LogoUrl { get; set; } // URL to the brand's logo image
        public ICollection<Product> Products { get; set; } = new List<Product>(); // Products associated with this brand
    }
}

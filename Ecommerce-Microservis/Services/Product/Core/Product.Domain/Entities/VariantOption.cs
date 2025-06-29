using Ecommerce.Base.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain.Entities
{
    public class VariantOption : BaseEntity
    {
        public string Name { get; set; } // The name of the variant option (e.g., "Color", "Size")
    }
}

using Ecommerce.Base.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain.Entities
{
    public class VariantOptionValue : BaseEntity
    {
        public Guid VariantOptionId { get; set; }
        public VariantOption VariantOption { get; set; }
        public string Value { get; set; } // e.g., "Red", "Large"
    }
}

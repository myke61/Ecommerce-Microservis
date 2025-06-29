using Ecommerce.Base.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain.Entities
{
    public class ProductVariantOption : BaseEntity
    {
        public Guid ProductVariantId { get; set; }
        public ProductVariant ProductVariant { get; set; }
        public Guid VariantOptionId { get; set; }
        public VariantOption VariantOption { get; set; }
        public Guid VariantOptionValueId { get; set; }
        public VariantOptionValue VariantOptionValue { get; set; }
    }
}

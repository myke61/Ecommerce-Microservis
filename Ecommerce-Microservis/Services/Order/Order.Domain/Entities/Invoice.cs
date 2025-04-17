using Ecommerce.Base.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Entities
{
    public class Invoice : BaseEntity
    {
        public SalesOrder SalesOrder { get; set; }
        public Guid SalesOrderId { get; set; }
        public decimal TotalPrice { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceCode { get; set; }
    }
}

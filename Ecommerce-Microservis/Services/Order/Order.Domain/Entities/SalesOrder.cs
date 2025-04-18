using Ecommerce.Base.Entities;
using Order.Domain.Enums;

namespace Order.Domain.Entities
{
    public class SalesOrder : BaseEntity
    {
        public Guid UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public ICollection<SalesOrderProduct>? SalesOrderProduct { get; set; }
    }
}

using Ecommerce.Base.Entities;

namespace Order.Domain.Entities
{
    public class SalesOrder : BaseEntity
    {
        public Guid UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public ICollection<SalesOrderProduct>? SalesOrderProduct { get; set; }
    }
}

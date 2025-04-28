using Order.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Order.Application.Responses.GetOrders
{
    public class GetOrdersResponse
    {
        public List<SalesOrderResponseDto> SalesOrders { get; set; }
    }

    public class SalesOrderResponseDto
    {
        public Guid Id { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public List<SalesOrderProductResponseDto> Products { get; set; }
    }

    public class SalesOrderProductResponseDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}

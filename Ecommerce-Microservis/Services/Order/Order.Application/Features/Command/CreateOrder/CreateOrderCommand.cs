using MediatR;
using Order.Application.Responses.CreateOrder;
using Order.Domain.Entities;
using Order.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Features.Command.CreateOrder
{
    public class CreateOrderCommand : IRequest<CreateOrderResponse>
    {
        public Guid UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public List<SalesOrderProductCommand> SalesOrderProductCommand { get; set; }
    }

    public class SalesOrderProductCommand
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}

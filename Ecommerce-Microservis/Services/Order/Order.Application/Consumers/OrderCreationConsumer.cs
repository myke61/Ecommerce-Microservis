using MassTransit;
using Order.Domain.Events;
using MediatR;
using Order.Application.Features.Command.CreateOrder;
using Order.Domain.Entities;

namespace Order.Application.Consumers
{
    public class OrderCreationConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly IMediator _mediator;
        public OrderCreationConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var request = new CreateOrderCommand
            {
                UserId = context.Message.UserId,
                TotalPrice = context.Message.TotalPrice,
                OrderStatus = Domain.Enums.OrderStatus.OPEN,
                SalesOrderProductCommand = [.. context.Message.BasketItems.Select(x => new SalesOrderProductCommand
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    UnitPrice = x.UnitPrice,
                    Quantity = x.Quantity
                })]
            };
            await _mediator.Send(request);
        }
    }
}

using MassTransit;
using Order.Domain.Events;
using MediatR;

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
            var orderCreatedEvent = context.Message;
        }
    }
}

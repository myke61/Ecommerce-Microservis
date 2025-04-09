using EventBus.Messages.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Handler
{
    public interface IIntegrationEventHandler<TIntegrationEvent> : IntegrationEventHandler where TIntegrationEvent : BaseIntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }

    public interface IntegrationEventHandler
    {

    }
}

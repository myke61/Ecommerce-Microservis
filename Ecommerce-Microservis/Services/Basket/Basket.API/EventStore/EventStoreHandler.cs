
using Basket.API.Entities;
using EventStore.Client;
using System.Text.Json;

namespace Basket.API.EventStore
{
    public class EventStoreHandler : IEventStoreHandler
    {
        private readonly EventStoreClient _storeClient;
        public EventStoreHandler(EventStoreClient storeClient)
        {
            _storeClient = storeClient;
        }
        public void AppendToStreamAsync<T>(string streamName,string type ,T data, CancellationToken cancellationToken = default) where T : class
        {
            var eventData = new EventData(
                Uuid.NewUuid(),
                type,
                JsonSerializer.SerializeToUtf8Bytes(data)
            );
             _storeClient.AppendToStreamAsync(
                streamName,
                StreamState.Any,
                [eventData]
            );
        }
    }
}

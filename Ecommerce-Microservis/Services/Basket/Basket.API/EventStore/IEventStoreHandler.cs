namespace Basket.API.EventStore
{
    public interface IEventStoreHandler
    {
        void AppendToStreamAsync<T>(string streamName, string type, T data, CancellationToken cancellationToken = default) where T : class;
    }
}

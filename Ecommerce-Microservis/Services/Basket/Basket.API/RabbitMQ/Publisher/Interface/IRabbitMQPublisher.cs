namespace Basket.API.RabbitMQ.Publisher.Interface
{
    public interface IRabbitMQPublisher
    {
        Task PublishMessageAsync<T>(T message,string queueName);
    }
}

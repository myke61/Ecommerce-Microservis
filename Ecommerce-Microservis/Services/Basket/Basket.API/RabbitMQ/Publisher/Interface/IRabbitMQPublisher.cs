namespace Basket.API.RabbitMQ.Publisher.Interface
{
    public interface IRabbitMQPublisher
    {
        Task PublishMessageAsync(string message,string queueName);
    }
}

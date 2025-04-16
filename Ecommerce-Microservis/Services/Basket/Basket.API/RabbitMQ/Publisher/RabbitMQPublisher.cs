using Basket.API.RabbitMQ.Publisher.Interface;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Basket.API.RabbitMQ.Publisher
{
    public class RabbitMQPublisher : IRabbitMQPublisher
    {
        private readonly RabbitMQSettings _settings;

        public RabbitMQPublisher(IOptions<RabbitMQSettings> rabbitMQSettings)
        {
            _settings = rabbitMQSettings.Value;
        }

        public async Task PublishMessageAsync(string message, string queueName)
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.HostName,
                Port = _settings.Port,
                UserName = _settings.UserName,
                Password = _settings.Password,
            };

            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();
            await channel.QueueDeclareAsync(queueName, durable: true, exclusive: false, autoDelete: false);

            var messageBody = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(messageBody);

            // Specify the type argument explicitly for BasicPublishAsync
            await channel.BasicPublishAsync<BasicProperties>(
                exchange: string.Empty,
                routingKey: queueName,
                mandatory: false,
                basicProperties: new BasicProperties(), // Provide an instance of BasicProperties
                body: body
            );
        }
    }
}

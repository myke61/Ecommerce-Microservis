using Basket.API.Context;
using Basket.API.Entities;
using Basket.API.RabbitMQ.Publisher.Interface;
using Basket.API.RabbitMQ.Queues;
using Microsoft.EntityFrameworkCore;

namespace Basket.API.Outbox
{
    public class OutboxWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IRabbitMQPublisher _rabbitMQ;
        ///private readonly BasketDbContext _dbContext;

        public OutboxWorker(IServiceScopeFactory scopeFactory, IRabbitMQPublisher rabbitMQPublisher)
        {
            _scopeFactory = scopeFactory;
            _rabbitMQ = rabbitMQPublisher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var _dbContext = scope.ServiceProvider.GetRequiredService<BasketDbContext>();
                    var outboxMessages = await _dbContext.OutboxMessage
                        .Where(x => !x.Processed)
                        .ToListAsync(stoppingToken);

                    foreach (var message in outboxMessages)
                    {
                        var body = Newtonsoft.Json.JsonConvert.DeserializeObject<Checkout>(message.Payload);
                        await _rabbitMQ.PublishMessageAsync(body, RabbitMQQueues.OrderCreationQueue);
                        message.Processed = true;
                    }

                    await _dbContext.SaveChangesAsync(stoppingToken);
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in OutboxWorker: {ex.Message}");
                }
            }

        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using Order.Application.Consumers;

namespace Order.Infastructure.MassTransit
{
    public static class MassTransitConfiguration
    {
        public static IServiceCollection AddMassTransitWithRabbitMQ(this IServiceCollection services)
        {
            var rabbitMQSettings = new RabbitMQSettings();
            services.AddSingleton(rabbitMQSettings);

            services.AddMassTransit<IPrimaryBus>(config =>
            {
                config.AddConsumer<OrderCreationConsumer>();
                config.UsingRabbitMq((ctx,cfg) =>
                {
                    cfg.Host(rabbitMQSettings.Primary.Host, 5672, "/", h =>
                    {
                        h.Username(rabbitMQSettings.Primary.Username);
                        h.Password(rabbitMQSettings.Primary.Password);
                    });
                    cfg.ReceiveEndpoint("OrderCreationQueue", e =>
                    {
                        e.ConfigureConsumeTopology = false;
                        e.DefaultContentType = new System.Net.Mime.ContentType("application/json");
                        e.UseRawJsonDeserializer();
                        //e.UseJsonDeserializer();
                        e.ConfigureConsumer<OrderCreationConsumer>(ctx);
                    });
                    cfg.ConfigureEndpoints(ctx);
                });
            });
            return services;
        }
    }
}

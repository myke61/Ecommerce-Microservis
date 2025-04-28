using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Order.Application.Consumers;
using System.Reflection;

namespace Order.Application
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceRegistration).Assembly));
            services.AddAutoMapper(typeof(ServiceRegistration).Assembly);
            return services;
        }
    }
}

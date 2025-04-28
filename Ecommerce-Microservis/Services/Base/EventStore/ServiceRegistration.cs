using EventStore.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventStore
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddEventStore(this IServiceCollection services)
        {
            services.AddScoped<IEventStoreHandler, EventStoreHandler>();
            return services;
        }
    }
}

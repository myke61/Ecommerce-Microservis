using Caching.Redis.Interface;
using Caching.Redis.Service;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Caching.Redis
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddRedisCache(this IServiceCollection services)
        {
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost:6379"));
            services.AddScoped<IRedisCache,RedisCacheService>();
            return services;
        }
    }
}

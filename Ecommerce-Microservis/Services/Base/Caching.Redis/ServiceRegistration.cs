using Caching.Redis.Interface;
using Caching.Redis.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caching.Redis
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddRedisCache(this IServiceCollection services,IConfiguration configuration)
        {
            var conn = configuration.GetConnectionString("Redis");
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(conn));
            services.AddScoped<IRedisCache, RedisCacheService>();
            return services;
        }
    }
}

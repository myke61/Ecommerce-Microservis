using Logging.Middleware;
using Logging.Options;
using Logging.Services.Interface;
using Logging.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Logging
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddElasticLogging(this IServiceCollection services, IConfiguration configuration)
        {
            ElasticSearchOptions elasticSearchOptions = new ElasticSearchOptions();
            elasticSearchOptions.Uri = configuration["ElasticSearch:Uri"];
            elasticSearchOptions.DefaultIndex = configuration["ElasticSearch:Index"];
            services.AddSingleton<IElasticSearchService, ElasticSearchService>();
            return services;
        }

        public static IApplicationBuilder UseElasticRequestLogging(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ElasticRequestLoggingMiddleware>();
        }
    }
}

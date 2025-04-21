using Logging.Models;
using Logging.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace Logging.Middleware
{
    public class ElasticRequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public ElasticRequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IElasticSearchService elasticService)
        {
            context.Request.EnableBuffering(); // Enable buffering to read the request body multiple times
            var body = string.Empty;
            if (context.Request.ContentLength > 0 && context.Request.ContentType?.Contains("application/json") == true)
            {
                using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
                body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            var log = new RequestLogModel
            {
                Method = context.Request.Method,
                Path = context.Request.Path,
                Query = context.Request.QueryString.ToString(),
                Body = body,
                Timestamp = DateTime.UtcNow
            };

            await elasticService.IndexAsync(log);

            await _next(context);
        }
    }
}

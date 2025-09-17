using LoginService.LoginService;
using Microsoft.Extensions.DependencyInjection;

namespace LoginService
{
    public static class ServiceRegistiration
    {
        public static IServiceCollection AddLoginService(this IServiceCollection services)
        {
            services.AddScoped<ILoginService, LoginService.LoginService>();
            return services;
        }
    }
}

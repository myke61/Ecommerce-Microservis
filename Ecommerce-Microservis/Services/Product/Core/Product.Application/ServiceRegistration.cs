using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Product.Application.Mapper;
using System.Reflection;

namespace Product.Application
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddAutoMapper(typeof(GeneralMapper).Assembly);
            return services;
        }
    }
}

using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Product.Application.Features.Queries.GetAllProduct;
using Product.Application.Features.Queries.GetProductById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Product.Application
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            //services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddAutoMapper(typeof(ServiceRegistration).Assembly);
            return services;
        }
    }
}

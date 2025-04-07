using Ecommerce.Base.Repositories.Interface;
using Ecommerce.Base.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Base
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddBaseServices(this IServiceCollection services)
        {
            //services.AddScoped<DbContext>();
            services.AddScoped(typeof(IQueryRepository<>), typeof(QueryRepository<>));
            services.AddScoped(typeof(ICommandRepository<>), typeof(CommandRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddScoped<DbContext, BasketDbContext>();
            //services.AddScoped<DbContext>();
            //services.AddScoped<IDbContextOptions>();
            return services;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Order.Persistance.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Persistance
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
        {
            services.AddScoped<DbContext, OrderDbContext>();
            return services;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Product.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Persistence
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
        {
            services.AddDbContext<ProductDbContext>(options => options.UseSqlServer(@"Data Source=DESKTOP-6ND9QO1\SQLEXPRESS;initial Catalog=ProductDB;integrated Security=true;TrustServerCertificate=True"));
            services.AddScoped<DbContext, ProductDbContext>();
            return services;
        }
    }
}

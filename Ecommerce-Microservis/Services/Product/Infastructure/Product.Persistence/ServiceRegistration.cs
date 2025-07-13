using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Product.Persistence.Context;

namespace Product.Persistence
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ProductDb");
            services.AddDbContext<ProductDbContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<DbContext, ProductDbContext>();
            return services;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Basket.API.Context
{
    public class BasketDbContextFactory : IDesignTimeDbContextFactory<BasketDbContext>
    {
        public BasketDbContext CreateDbContext(string[] args) 
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<BasketDbContext>();
            var connString = configuration.GetConnectionString("PostgreSQL");
            optionsBuilder.UseNpgsql(connString);
            return new BasketDbContext(optionsBuilder.Options);
        }
    }
}

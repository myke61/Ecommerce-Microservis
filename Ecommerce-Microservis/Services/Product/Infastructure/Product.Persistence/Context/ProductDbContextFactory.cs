using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Product.Persistence.Context;

public class ProductDbContextFactory : IDesignTimeDbContextFactory<ProductDbContext>
{
    public ProductDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProductDbContext>();

        var connectionString = "Data Source=HPYZSG3\\SQLEXPRESS01;initial Catalog=ProductDB;integrated Security=true;TrustServerCertificate=True";
        optionsBuilder.UseSqlServer(connectionString);

        return new ProductDbContext(optionsBuilder.Options);
    }
}

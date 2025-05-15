using Ecommerce.Base.Entities;
using Microsoft.EntityFrameworkCore;
using Order.Domain.Entities;

namespace Order.Persistance.Context
{
    public class OrderDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=HPYZSG3\\SQLEXPRESS01;initial Catalog=OrderDB;integrated Security=true;TrustServerCertificate=True");
        }
        public OrderDbContext()
        {

        }
        public OrderDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<SalesOrder> SalesOrder { get; set; }
        public DbSet<SalesOrderProduct> SalesOrderProduct { get; set; }
        public DbSet<Invoice> Invoice { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var datas = ChangeTracker.Entries<BaseEntity>();

            foreach (var data in datas)
            {
                _ = data.State switch
                {
                    EntityState.Added => data.Entity.CreatedDate = DateTime.UtcNow,
                    EntityState.Modified => data.Entity.UpdatedDate = DateTime.UtcNow,
                    _ => DateTime.UtcNow
                };
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}

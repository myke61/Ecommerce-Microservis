using Ecommerce.Base.Entities;
using Microsoft.EntityFrameworkCore;

namespace Product.Persistence.Context
{
    public class ProductDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=HPYZSG3\\SQLEXPRESS01;initial Catalog=ProductDB;integrated Security=true;TrustServerCertificate=True");
        }
        public ProductDbContext()
        {

        }
        public ProductDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Domain.Entities.Product> Product { get; set; }

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            DateTime date = DateTime.Today;
            var Iphone13 = new Domain.Entities.Product("Iphone13White","Iphone 13", "Phone", "https://productimages.hepsiburada.net/s/189/424-600/110000155170588.jpg/format:webp", 34299.0m);
            var Iphone14 = new Domain.Entities.Product("Iphone14White", "Iphone 14", "Phone", "https://productimages.hepsiburada.net/s/376/424-600/110000393677091.jpg/format:webp", 38299.0m);
            var Iphone15 = new Domain.Entities.Product("Iphone15White", "Iphone 15", "Phone", "https://productimages.hepsiburada.net/s/462/424-600/110000498573428.jpg/format:webp", 48299.0m);
            Iphone13.Id = new Guid("39ecb0f9-e945-48ea-9880-8400d61cb7c9");
            Iphone13.CreatedDate = date;
            Iphone13.UpdatedDate = date;
            Iphone14.Id = new Guid("63870905-124c-41be-9827-a9e026172fb6");
            Iphone14.CreatedDate = date;
            Iphone14.UpdatedDate = date;
            Iphone15.Id = new Guid("60742fba-3da2-42ec-931a-7e7edd77d3a5");
            Iphone15.CreatedDate = date;
            Iphone15.UpdatedDate = date;

            modelBuilder.Entity<Domain.Entities.Product>(entity => {
                entity.HasIndex(e => e.Code).IsUnique();
            });

            base.OnModelCreating(modelBuilder);
            _ = modelBuilder.Entity<Domain.Entities.Product>().HasData(
                Iphone13,
                Iphone14,
                Iphone15
            );
        }
    }
}

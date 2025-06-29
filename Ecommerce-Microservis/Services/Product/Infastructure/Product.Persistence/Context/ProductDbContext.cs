using Ecommerce.Base.Entities;
using Microsoft.EntityFrameworkCore;
using Product.Domain.Entities;

namespace Product.Persistence.Context
{
    public class ProductDbContext : DbContext
    {
        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=HPYZSG3\\SQLEXPRESS01;initial Catalog=ProductDB;integrated Security=true;TrustServerCertificate=True");
        }*/
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {
        }

        public DbSet<Product.Domain.Entities.Product> Products { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<VariantOption> VariantOptions { get; set; }
        public DbSet<VariantOptionValue> VariantOptionValues { get; set; }
        public DbSet<ProductVariantOption> ProductVariantOptions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Domain.Entities.Product>()
                .HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId);

            modelBuilder.Entity<Domain.Entities.Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<Category>()
                .HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId);

            modelBuilder.Entity<ProductVariantOption>()
                .HasOne(pvo => pvo.ProductVariant)
                .WithMany(pv => pv.VariantOptions)
                .HasForeignKey(pvo => pvo.ProductVariantId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductVariantOption>()
                .HasOne(pvo => pvo.VariantOption)
                .WithMany()
                .HasForeignKey(pvo => pvo.VariantOptionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductVariantOption>()
                .HasOne(pvo => pvo.VariantOptionValue)
                .WithMany()
                .HasForeignKey(pvo => pvo.VariantOptionValueId)
                .OnDelete(DeleteBehavior.Restrict);
        }

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

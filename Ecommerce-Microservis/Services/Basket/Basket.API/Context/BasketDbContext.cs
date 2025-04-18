using Basket.API.Outbox;
using Ecommerce.Base.Entities;
using Microsoft.EntityFrameworkCore;

namespace Basket.API.Context
{
    public class BasketDbContext : DbContext
    {
        public BasketDbContext(DbContextOptions<BasketDbContext> options) : base(options)
        {
        }
        public DbSet<OutboxMessage> OutboxMessage { get; set; }
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

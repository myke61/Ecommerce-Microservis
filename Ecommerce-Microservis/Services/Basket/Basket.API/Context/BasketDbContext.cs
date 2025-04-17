using Basket.API.Outbox;
using Microsoft.EntityFrameworkCore;

namespace Basket.API.Context
{
    public class BasketDbContext : DbContext
    {
        public BasketDbContext(DbContextOptions<BasketDbContext> options) : base(options)
        {
        }
        public DbSet<OutboxMessage> OutboxMessage { get; set; }
    }
}

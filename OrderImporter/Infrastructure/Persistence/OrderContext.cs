using Microsoft.EntityFrameworkCore;
using OrderImporter.Infrastructure.Persistence.Entities;

namespace OrderImporter.Infrastructure.Persistence
{
    public sealed class OrderContext(DbContextOptions<OrderContext> options) : DbContext(options)
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderError> OrderErrors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderErrorConfiguration());
        }
    }
}

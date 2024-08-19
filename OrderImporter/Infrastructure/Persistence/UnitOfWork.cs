using OrderImporter.Infrastructure.Persistence.Entities;
using OrderImporter.Infrastructure.Persistence.Repositories;

namespace OrderImporter.Infrastructure.Persistence
{  

    public sealed class UnitOfWork(OrderContext context, IRepository<Order> orderRepository, IRepository<OrderError> orderErrors) : IUnitOfWork
    {
        public IRepository<Order> Orders { get; } = orderRepository;
        public IRepository<OrderError> OrderErrors { get; } = orderErrors;

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
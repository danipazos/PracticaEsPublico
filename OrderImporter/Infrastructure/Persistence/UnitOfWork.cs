using OrderImporter.Infrastructure.Persistence.Entities;
using OrderImporter.Infrastructure.Persistence.Repositories;

namespace OrderImporter.Infrastructure.Persistence
{  

    internal sealed class UnitOfWork : IUnitOfWork
    {
        private readonly OrderContext _context;
        public IRepository<Order> Orders { get; }
        public IRepository<OrderError> OrderErrors { get; }

        public UnitOfWork(OrderContext context, IRepository<Order> orderRepository, IRepository<OrderError> orderErrors)
        {
            _context = context;
            Orders = orderRepository;
            OrderErrors = orderErrors;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
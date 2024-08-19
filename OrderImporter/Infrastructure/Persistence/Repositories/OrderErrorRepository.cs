using OrderImporter.Infrastructure.Persistence.Entities;

namespace OrderImporter.Infrastructure.Persistence.Repositories
{
    internal class OrderErrorRepository : IRepository<OrderError>
    {
        private readonly OrderContext _context;

        public OrderErrorRepository(OrderContext context)
        {
            _context = context;
        }

        public async Task AddRangeAsync(IEnumerable<OrderError> orderErrors)
        {
            await _context.OrderErrors.AddRangeAsync(orderErrors);
        }

        public IEnumerable<OrderError> GetAllAsync()
        {
            return _context.OrderErrors;
        }
    }
}
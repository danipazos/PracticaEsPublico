using OrderImporter.Infrastructure.Persistence.Entities;

namespace OrderImporter.Infrastructure.Persistence.Repositories
{
    internal class OrderRepository : IRepository<Order>
    {
        private readonly OrderContext _context;

        public OrderRepository(OrderContext context)
        {
            _context = context;
        }

        public async Task AddRangeAsync(IEnumerable<Order> orders)
        {
            await _context.Orders.AddRangeAsync(orders);
        }

        public IEnumerable<Order> GetAllAsync()
        {
            return _context.Orders;
        }
    }
}
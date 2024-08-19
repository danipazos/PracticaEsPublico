using OrderImporter.Infrastructure.Persistence.Entities;

namespace OrderImporter.Infrastructure.Persistence.Repositories
{
    public sealed class OrderRepository(OrderContext context) : IRepository<Order>
    {
        public async Task AddRangeAsync(IEnumerable<Order> orders)
        {
            await context.Orders.AddRangeAsync(orders);
        }

        public IEnumerable<Order> GetAllAsync()
        {
            return context.Orders;
        }
    }
}
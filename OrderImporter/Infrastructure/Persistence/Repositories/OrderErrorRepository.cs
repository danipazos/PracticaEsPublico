using OrderImporter.Infrastructure.Persistence.Entities;

namespace OrderImporter.Infrastructure.Persistence.Repositories
{
    public sealed class OrderErrorRepository(OrderContext context) : IRepository<OrderError>
    {
        public async Task AddRangeAsync(IEnumerable<OrderError> orderErrors)
        {
            await context.AddRangeAsync(orderErrors);
        }

        public IEnumerable<OrderError> GetAll()
        {
            return context.OrderErrors;
        }
    }
}
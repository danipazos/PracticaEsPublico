using OrderImporter.Infrastructure.Persistence.Entities;

namespace OrderImporter.Infrastructure.Persistence.Repositories
{
    public sealed class OrderErrorRepository(OrderContext context) : IRepository<OrderError>
    {
        public async Task AddRangeAsync(IEnumerable<OrderError> orderErrors)
        {
            await context.OrderErrors.AddRangeAsync(orderErrors);
        }

        public IEnumerable<OrderError> GetAllAsync()
        {
            return context.OrderErrors;
        }
    }
}
using OrderImporter.Common.Log;
using OrderImporter.Infrastructure.Persistence.Entities;
using OrderImporter.Infrastructure.Persistence.Repositories;

namespace OrderImporter.Infrastructure.Persistence
{

    public sealed class UnitOfWork(OrderContext context, IRepository<Order> orderRepository, IRepository<OrderError> orderErrors, ILog log) : IUnitOfWork
    {
        public IRepository<Order> Orders { get; } = orderRepository;
        public IRepository<OrderError> OrderErrors { get; } = orderErrors;

        public async Task SaveChangesAsync()
        {
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                log.Error($"Se ha producido un error al guardar los datos: \n {ex.Message}");
            }
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
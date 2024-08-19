using OrderImporter.Infrastructure.Persistence.Entities;
using OrderImporter.Infrastructure.Persistence.Repositories;

namespace OrderImporter.Infrastructure.Persistence
{   
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync();

        IRepository<Order> Orders { get; }
        IRepository<OrderError> OrderErrors { get; }
    }
}
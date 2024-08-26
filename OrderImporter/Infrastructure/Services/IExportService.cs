using OrderImporter.Infrastructure.Persistence.Entities;

namespace OrderImporter.Infrastructure.Services
{
    public interface IExportService
    {
        Task ExportOrdersAsync(IEnumerable<Order> orders);
    }
}
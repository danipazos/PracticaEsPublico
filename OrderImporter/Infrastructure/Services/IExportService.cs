using OrderImporter.Domain.Models;
using OrderImporter.Infrastructure.Persistence.Entities;

namespace OrderImporter.Infrastructure.Services
{
    internal interface IExportService
    {
        Task ExportOrdersAsync(IEnumerable<Order> orders);
        Task ExportOrderTotalsAsync(List<OrderTotal> groupedTotals);
    }
}
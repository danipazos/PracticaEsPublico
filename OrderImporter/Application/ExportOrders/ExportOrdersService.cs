using OrderImporter.Infrastructure.Persistence.Entities;
using OrderImporter.Infrastructure.Persistence.Repositories;
using OrderImporter.Infrastructure.Services;

namespace OrderImporter.Application.ExportOrders
{
    public sealed class ExportOrdersService(IRepository<Order> orderRepository, IExportService exportSevice) : IExportOrdersService
    {
        public async Task ExportOrdersAsync()
        {
            var orders = orderRepository.GetAll();
            await exportSevice.ExportOrdersAsync(orders.OrderBy(order => order.Id));
        }
    }
}
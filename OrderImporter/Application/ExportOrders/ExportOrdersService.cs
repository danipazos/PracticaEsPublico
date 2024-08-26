using OrderImporter.Common.Log;
using OrderImporter.Infrastructure.Persistence.Entities;
using OrderImporter.Infrastructure.Persistence.Repositories;
using OrderImporter.Infrastructure.Services;

namespace OrderImporter.Application.ExportOrders
{
    public sealed class ExportOrdersService(IRepository<Order> orderRepository, IExportService exportSevice, ILog log) : IExportOrdersService
    {
        public async Task ExportOrdersAsync()
        {
            log.Info("Iniciando la exportación de pedidos...");
            var orders = orderRepository.GetAll();

            log.Info($"Exportando {orders.Count()} pedidos.");
            await exportSevice.ExportOrdersAsync(orders.OrderBy(order => order.Id));
        }
    }
}
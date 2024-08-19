using OrderImporter.Common.Exceptions;
using OrderImporter.Domain.Models;
using OrderImporter.Infrastructure.Persistence.Entities;
using OrderImporter.Infrastructure.Persistence.Repositories;
using OrderImporter.Infrastructure.Services;
using System.Configuration;

namespace OrderImporter.Application.ExportOrders
{
    public sealed class ExportOrdersService(IRepository<Order> orderRepository, IExportService exportSevice) : IExportOrdersService
    {
        public async Task ExportOrdersAsync()
        {
            var orders = orderRepository.GetAllAsync();
            await exportSevice.ExportOrdersAsync(orders.OrderBy(order => order.Id));

            var orderGroupedTotals = GetTotals(orders);
            await exportSevice.ExportOrderTotalsAsync(orderGroupedTotals);
        }

        private List<OrderTotal> GetTotals(IEnumerable<Order> orders)
        {
            string propertiesToOrderBy = ConfigurationManager.AppSettings["TotalProperties"];
            string[] groupByProperties = propertiesToOrderBy.Split('|');

            var result = new List<OrderTotal>();

            foreach (var property in groupByProperties)
            {
                try
                {
                    var groups = orders.GroupBy(order => order.GetTypeValue(property));

                    var groupCounts = groups
                        .ToDictionary(g => g.Key.ToString(), g => g.Count())
                        .OrderBy(order => order.Key)
                        .ToDictionary(x => x.Key, x => x.Value);

                    result.AddRange(groupCounts
                        .Select(group => new OrderTotal { Property = property, Value = group.Key, Total = group.Value }));
                }
                catch (NullReferenceException nex)
                {
                    throw new OrderImporterException($"Error al calcular los totales. El campo {property} no existe");
                }
            }

            return result;
        }
    }
}
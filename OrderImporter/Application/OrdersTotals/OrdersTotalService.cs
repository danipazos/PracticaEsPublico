using OrderImporter.Common.Configuration;
using OrderImporter.Common.Exceptions;
using OrderImporter.Common.Log;
using OrderImporter.Infrastructure.Persistence.Entities;
using OrderImporter.Infrastructure.Persistence.Repositories;

namespace OrderImporter.Application.OrdersTotals
{
    public sealed class OrdersTotalService(IRepository<Order> orderRepository, IAppConfig appConfig, ILog log) : IOrdersTotalService
    {
        public Dictionary<string, Dictionary<string, int>> GetTotals()
        {
            log.Info("Iniciando el calculo de totales de pedidos...");
            var orders = orderRepository.GetAll();

            var result = new Dictionary<string, Dictionary<string, int>>();

            foreach (var property in appConfig.PropertiesToGroupBy)
            {
                var propertyInfo = typeof(Order).GetProperty(property);
                if (propertyInfo == null)
                {
                    throw new OrderImporterException($"Error al calcular los totales. El campo {property} no existe");
                }

                try
                {
                    var ordersGroupedByProperty = orders
                        .GroupBy(o => propertyInfo.GetValue(o)?.ToString() ?? "")
                        .OrderBy(g => g.Key)
                        .ToDictionary(g => g.Key, g => g.Count(), StringComparer.OrdinalIgnoreCase);

                    result.Add(property, ordersGroupedByProperty);
                }
                catch (Exception ex) when (ex is not OrderImporterException)
                {
                    throw new OrderImporterException($"Error al calcular los totales para el campo {property}: {ex.Message}");
                }
            }

            return result;
        }
    }
}

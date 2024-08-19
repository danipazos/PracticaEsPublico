using CsvHelper;
using OrderImporter.Common.Exceptions;
using OrderImporter.Domain.Models;
using OrderImporter.Infrastructure.Mapping;
using OrderImporter.Infrastructure.Persistence.Entities;
using System.Configuration;
using System.Globalization;

namespace OrderImporter.Infrastructure.Services
{
    internal class ExportToCsvService : IExportService
    {
        public async Task ExportOrdersAsync(IEnumerable<Order> orders)
        {
            try
            {
                string fileName = ConfigurationManager.AppSettings["OrderCsvFileName"];
                using var writer = new StreamWriter(fileName);
                using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

                csv.Context.RegisterClassMap<OrderClassMap>();
                
                csv.WriteHeader<Order>();
                await csv.NextRecordAsync();
                await csv.WriteRecordsAsync(orders);

                writer.Flush();
            }
            catch (IOException ioex)
            {
                throw new OrderImporterException($"Error al gestionar el fichero para la exportación de datos: {ioex.Message}");

            }
            catch (Exception ex)
            {
                throw new OrderImporterException($"Error al generar la exportación de datos:{ ex.Message}");
            }
        }
                
        public async Task ExportOrderTotalsAsync(List<OrderTotal> groupedTotals)
        {
            try
            {
                string fileName = ConfigurationManager.AppSettings["OrderTotalCsvFileName"];
                using var writer = new StreamWriter(fileName);
                using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

                csv.Context.RegisterClassMap<OrderTotalClassMap>();
                
                csv.WriteHeader<OrderTotal>();
                await csv.NextRecordAsync();
                await csv.WriteRecordsAsync(groupedTotals);

                writer.Flush();
            }
            catch (IOException ioex)
            {
                throw new OrderImporterException($"Error al gestionar el fichero para la exportación de datos: {ioex.Message}");

            }
            catch (Exception ex)
            {
                throw new OrderImporterException($"Error al generar la exportación de datos: {ex.Message}");
            }
        }
    }
}

using CsvHelper;
using OrderImporter.Common.Configuration;
using OrderImporter.Common.Exceptions;
using OrderImporter.Infrastructure.Mapping;
using OrderImporter.Infrastructure.Persistence.Entities;
using System.Globalization;

namespace OrderImporter.Infrastructure.Services
{
    public sealed class ExportToCsvService(IAppConfig appConfig) : IExportService
    {
        public async Task ExportOrdersAsync(IEnumerable<Order> orders)
        {
            try
            {
                using var writer = new StreamWriter(appConfig.OrderCsvFileName);
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
                throw new OrderImporterException($"Error al generar la exportación de datos:{ex.Message}");
            }
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using OrderImporter.Application.ExportOrders;
using OrderImporter.Application.OrderImport;
using OrderImporter.Application.OrdersTotals;
using OrderImporter.Common.Configuration;
using OrderImporter.Common.Exceptions;
using OrderImporter.Common.Log;

namespace OrderImporter
{
    public static class Program
    {
        static async Task Main(string[] args)
        {
            var host = DIConfiguration.CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            var log = services.GetRequiredService<ILog>();
            var appConfig = services.GetRequiredService<IAppConfig>();
            var importService = services.GetRequiredService<IImportOrdersService>();
            var exportService = services.GetRequiredService<IExportOrdersService>();
            var orderTotalsService = services.GetRequiredService<IOrdersTotalService>();

            try
            {                
                await importService.ImportOrdersAsync();

                await exportService.ExportOrdersAsync();
                                
                var orderTotals = orderTotalsService.GetTotals();
                ShowTotalsInConsole(orderTotals);

                log.Info($"Consulte el fichero exportado en la ruta: {AppDomain.CurrentDomain.BaseDirectory}{Path.DirectorySeparatorChar}{appConfig.OrderCsvFileName}");
            }
            catch (OrderImporterException aex)
            {
                log.Error($"Ha ocurrido un error! \n {aex.Message}");
            }
            catch (Exception ex)
            {
                log.Error($"Ha ocurrido un error no controlado! \n {ex.Message}");
            }
            finally
            {
                Console.ReadKey();
            }
        }

        private static void ShowTotalsInConsole(Dictionary<string, Dictionary<string, int>> groupedOrderByProperties)
        {            
            foreach (var groupProperty in groupedOrderByProperties)
            {
                Console.WriteLine(groupProperty.Key);
                Console.WriteLine("-----------------------------------------");
                foreach (var (key, value) in groupProperty.Value)
                {
                    Console.WriteLine($"    {key,-35} {value,7} pedidos");
                }
                Console.WriteLine();
            }
        }
    }
}
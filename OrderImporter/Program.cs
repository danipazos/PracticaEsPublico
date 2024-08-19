using Microsoft.Extensions.DependencyInjection;
using OrderImporter.Application.ExportOrders;
using OrderImporter.Application.OrderImport;
using OrderImporter.Common.Exceptions;

namespace OrderImporter
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var host = DIConfiguration.CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var importService = services.GetRequiredService<IImportOrdersService>();
                var exportService = services.GetRequiredService<IExportOrdersService>();

                Console.WriteLine("Iniciando la recuperación de pedidos.");
                await importService.ImportOrdersAsync();
                Console.WriteLine("Recuperación de pedidos finalizada.");

                Console.WriteLine("Iniciando la exportación de pedidos.");
                await exportService.ExportOrdersAsync();
                Console.WriteLine("Exportación de pedidos finalizada.");

                Console.WriteLine($"Consulte los ficheros exportados en la ruta {AppDomain.CurrentDomain.BaseDirectory}");
            }
            catch (OrderImporterException aex)
            {
                Console.WriteLine("**************************************************");
                Console.WriteLine($"Ha ocurrido un error! \n {aex.Message}");
                Console.WriteLine("**************************************************");
            }
            catch (Exception ex)
            {
                Console.WriteLine("**************************************************");
                Console.WriteLine($"Ha ocurrido un error no controlado! \n {ex.Message}");
                Console.WriteLine("**************************************************");
            }
            finally
            {
                Console.ReadKey();
            }
        }
    }
}
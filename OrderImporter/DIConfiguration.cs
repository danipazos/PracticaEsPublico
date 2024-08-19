using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderImporter.Application.ExportOrders;
using OrderImporter.Application.OrderImport;
using OrderImporter.Domain.Models;
using OrderImporter.Infrastructure.Persistence;
using OrderImporter.Infrastructure.Persistence.Entities;
using OrderImporter.Infrastructure.Persistence.Repositories;
using OrderImporter.Infrastructure.Services;
using Polly;
using Polly.Extensions.Http;
using System.Configuration;

namespace OrderImporter
{
    internal class DIConfiguration
    {
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services
                        .AddHttpClient<IDataSource<OrderDTO>, ApiDataSource>("OrderApiClient")
                        .AddPolicyHandler(GetRetryPolicy()).RemoveAllLoggers();
                    services.AddScoped<IExportService, ExportToCsvService>();

                    services.AddScoped<IImportOrdersService, ImportOrdersService>();
                    services.AddScoped<IExportOrdersService, ExportOrdersService>();

                    services.AddScoped<IUnitOfWork, UnitOfWork>();
                    services.AddScoped<IRepository<Order>, OrderRepository>();
                    services.AddScoped<IRepository<OrderError>, OrderErrorRepository>();
                   
                    services.AddDbContext<OrderContext>(options => options.UseInMemoryDatabase("OrdersDb"));
                });
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            int maxRetries = int.Parse(ConfigurationManager.AppSettings["Retries"]);
            int secondsforRetry = int.Parse(ConfigurationManager.AppSettings["SecondsBetweenRetries"]);
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(maxRetries, retryAttempt => TimeSpan.FromSeconds(secondsforRetry));
        }
    }
}

using OrderImporter.Common.Exceptions;
using OrderImporter.Common.Log;
using OrderImporter.Domain.Models;
using OrderImporter.Domain.Validations;
using OrderImporter.Infrastructure.Persistence;
using OrderImporter.Infrastructure.Persistence.Entities;
using OrderImporter.Infrastructure.Services;

namespace OrderImporter.Application.OrderImport
{
    public sealed class ImportOrdersService(IDataSource<OrderDTO> dataSource, IUnitOfWork unitOfWork, ILog log) : IImportOrdersService
    {
        public async Task ImportOrdersAsync()
        {
            try
            {
                await foreach (var orderResponses in dataSource.GetDataAsync())
                {
                    IEnumerable<Result<OrderModel>> orderResults = orderResponses.Select(CreateOrder);
                    await StoreOrdersInDbAsync(orderResults);
                }
            }
            catch (Exception ex)
            {
                throw new OrderImporterException($"Error al importar los datos: {ex.Message}");
            }
        }

        private Result<OrderModel> CreateOrder(OrderDTO orderResponse)
        {
            var validator = new OrderValidator();
            var order = OrderModel.Create(orderResponse);

            var validation = validator.Validate(order);
            return validation.IsValid ?
                Result<OrderModel>.Success(order) :
                Result<OrderModel>.Failure(order, validation.Errors.Select(error => error.ErrorMessage).ToList());
        }

        private async Task StoreOrdersInDbAsync(IEnumerable<Result<OrderModel>> ordersToStore)
        {
            var orders = ordersToStore.Select(order => Order.FromModel(order.Value));
            await unitOfWork.Orders.AddRangeAsync(orders);

            var orderErrors = ordersToStore
                .Where(order => order.IsFailure)
                .SelectMany(order => order.Errors
                    .Select(error => new OrderError { OrderId = order.Value.Id, Error = error }));
            if (orderErrors.Any())
            {
                await unitOfWork.OrderErrors.AddRangeAsync(orderErrors);
                log.Info($"{orderErrors.Count()} pedidos de los {ordersToStore.Count()} pedidos a guardar tienen errores.");
            }

            await unitOfWork.SaveChangesAsync();
        }
    }
}

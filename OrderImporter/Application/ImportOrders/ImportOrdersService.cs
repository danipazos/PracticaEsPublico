using OrderImporter.Common.Exceptions;
using OrderImporter.Domain.Models;
using OrderImporter.Domain.Validations;
using OrderImporter.Infrastructure.Persistence;
using OrderImporter.Infrastructure.Persistence.Entities;
using OrderImporter.Infrastructure.Services;
using System.Configuration;

namespace OrderImporter.Application.OrderImport
{
    internal sealed class ImportOrdersService : IImportOrdersService
    {
        private readonly IDataSource<OrderDTO> _dataSource;
        private readonly IUnitOfWork _unitOfWork;

        public ImportOrdersService(IDataSource<OrderDTO> dataSource, IUnitOfWork unitOfWork)
        {
            _dataSource = dataSource;
            _unitOfWork = unitOfWork;
        }

        public async Task ImportOrdersAsync()
        {
            int batchSize = int.Parse(ConfigurationManager.AppSettings["MaxBatchSize"]);
            var batch = new List<Result<OrderModel>>(batchSize);

            try
            {
                await foreach (var orderResponses in _dataSource.GetDataAsync())
                {
                    IEnumerable<Result<OrderModel>> orderResults = orderResponses.Select(CreateOrder);

                    batch.AddRange(orderResults);

                    if (batch.Count >= batchSize)
                    {
                        await StoreOrdersInDbAsync(batch);
                        batch.Clear();
                    }
                }

                if (batch.Any())
                {
                    await StoreOrdersInDbAsync(batch);
                }
            }
            catch (Exception ex)
            {               
                throw new OrderImporterException($"Error al importar los datos: { ex.Message}");
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

        private async Task StoreOrdersInDbAsync(List<Result<OrderModel>> ordersToStore)
        {
            var orders = ordersToStore.Select(order => Order.FromModel(order.Value));
            await _unitOfWork.Orders.AddRangeAsync(orders);

            var orderErrors = ordersToStore
                .Where(order => order.IsFailure)
                .SelectMany(order => order.Errors
                    .Select(error => new OrderError { OrderId = order.Value.Id, Error = error }))
                .ToList();
            if (orderErrors.Count > 0)
            {
                await _unitOfWork.OrderErrors.AddRangeAsync(orderErrors);
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}

using Newtonsoft.Json;
using OrderImporter.Common.Configuration;
using OrderImporter.Common.Log;
using OrderImporter.Domain.Models;

namespace OrderImporter.Infrastructure.Services
{
    public sealed class ApiDataSource : IDataSource<OrderDTO>
    {
        private readonly HttpClient _httpClient;
        private readonly ILog _log;
        private readonly int _pageSize;
        private readonly string _apiUrl;
        private readonly int _maxConcurrentRequests;
        private readonly SemaphoreSlim _semaphore;

        public ApiDataSource(HttpClient httpClient, IAppConfig appConfig, ILog log)
        {
            _httpClient = httpClient;
            _log = log;

            _pageSize = appConfig.PageSize;
            _apiUrl = appConfig.OrdersApiUrl;
            _maxConcurrentRequests = appConfig.MaxConcurrentRequests;
            _semaphore = new SemaphoreSlim(_maxConcurrentRequests, _maxConcurrentRequests);
        }

        public async IAsyncEnumerable<List<OrderDTO>> GetDataAsync()
        {
            int page = 1;
            int totalOrdersRetrieved = 0;
            bool hasMorePages = true;
            var startTime = DateTime.Now;

            _log.Info($"Iniciando recuperación de pedidos. Tamaño de página: {_pageSize}, Solicitudes concurrentes máximas: {_maxConcurrentRequests}");

            while (hasMorePages)
            {
                _log.Info($"Recuperando los pedidos desde la pagina {page} hasta {page + _maxConcurrentRequests}.");

                var tasks = new List<Task<OrderApiResponse>>();
                for (int i = 0; i < _maxConcurrentRequests && hasMorePages; i++)
                {
                    int currentPage = page + i;
                    tasks.Add(GetOrdersAsync(currentPage));
                }

                while (tasks.Count > 0)
                {
                    Task<OrderApiResponse> completedTask = await Task.WhenAny(tasks);
                    tasks.Remove(completedTask);

                    OrderApiResponse result = await completedTask;
                    if (result?.Content != null)
                    {
                        totalOrdersRetrieved += result.Content.Count;
                        yield return result.Content;
                    }

                    hasMorePages = result?.Links?.Next != null;
                }

                page += _maxConcurrentRequests;
            }

            _log.Info($"Recuperación de pedidos completada. Total de pedidos: {totalOrdersRetrieved}. Tiempo total: {(DateTime.Now - startTime).TotalSeconds:F2} segundos");
        }

        private async Task<OrderApiResponse> GetOrdersAsync(int currentPage)
        {
            string url = $"{_apiUrl}?page={currentPage}&max-per-page={_pageSize}";
            await _semaphore.WaitAsync();

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            _semaphore.Release();

            var result = JsonConvert.DeserializeObject<OrderApiResponse>(content);
            _log.Info($"Página {currentPage}: Recuperados {result.Content.Count} pedidos.");

            return result;
        }
    }
}
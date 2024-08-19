using Newtonsoft.Json;
using OrderImporter.Common.Exceptions;
using OrderImporter.Domain.Models;
using System.Configuration;

namespace OrderImporter.Infrastructure.Services
{
    public sealed class ApiDataSource(HttpClient httpClient) : IDataSource<OrderDTO>
    {
        private readonly int _pageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"]);
        private readonly string _apiUrl = ConfigurationManager.AppSettings["OrdersApiUrl"];
        private readonly int _maxConcurrentRequests = int.Parse(ConfigurationManager.AppSettings["MaxConcurrentRequests"]);

        public async IAsyncEnumerable<List<OrderDTO>> GetDataAsync()
        {
            int page = 0;

            while (true)
            {
                var tasks = new List<Task<OrderApiResponse>>();

                for (int i = 0; i < _maxConcurrentRequests; i++)
                {
                    var url = $"{_apiUrl}?page={page + i}&max-per-page={_pageSize}";
                    tasks.Add(GetOrdersAsync(url));
                }

                OrderApiResponse[] orderResults = await Task.WhenAll(tasks);

                foreach (var order in orderResults)
                {
                    yield return order.Content;
                    if (order?.Links?.Next == null)
                    {
                        yield break;
                    }
                }

                page += _maxConcurrentRequests;
                if (orderResults.All(order => order?.Links?.Next == null))
                {
                    yield break; 
                }
            }
        }

        private async Task<OrderApiResponse> GetOrdersAsync(string url)
        {
            try
            {
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<OrderApiResponse>(content);
       
            }
            catch (HttpRequestException ex)
            {
                throw new OrderImporterException($"Error al recuperar los datos de la api para la url {url}: {ex.Message}");
            }
        }
    }
}

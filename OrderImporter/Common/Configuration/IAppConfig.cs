namespace OrderImporter.Common.Configuration
{
    public interface IAppConfig
    {
        string OrdersApiUrl { get; }
        int PageSize { get; }
        int Retries { get; }
        int SecondsBetweenRetries { get; }
        string OriginDateFormat { get; }
        string ResultDateFormat { get; }
        int MaxConcurrentRequests { get; }
        string OrderCsvFileName { get; }
        string[] PropertiesToGroupBy { get; }
    }
}
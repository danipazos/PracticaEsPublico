namespace OrderImporter.Infrastructure.Services
{
    internal interface IDataSource<T>
    {
        IAsyncEnumerable<List<T>> GetDataAsync();
    }
}

namespace OrderImporter.Infrastructure.Services
{
    public interface IDataSource<T>
    {
        IAsyncEnumerable<List<T>> GetDataAsync();
    }
}

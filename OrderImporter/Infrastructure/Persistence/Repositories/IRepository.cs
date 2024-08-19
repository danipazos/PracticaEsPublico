namespace OrderImporter.Infrastructure.Persistence.Repositories
{
    internal interface IRepository<T> where T : class
    {
        Task AddRangeAsync(IEnumerable<T> entities);
        IEnumerable<T> GetAllAsync();
    }
}
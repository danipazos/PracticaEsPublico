namespace OrderImporter.Infrastructure.Persistence.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task AddRangeAsync(IEnumerable<T> entities);
        IEnumerable<T> GetAllAsync();
    }
}
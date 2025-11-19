namespace Core.Domain.Repository;

public interface ICollectionRepository
{
    IAsyncEnumerable<T> GetListAsync<T>() where T : class;
}
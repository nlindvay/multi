namespace multi.lib.common;

public interface IRepository<T>
{
    Task<IEnumerable<T>> Get(CancellationToken ct = default);
    Task<T?> Get(string id, CancellationToken ct = default);
    Task<bool> Upsert(T entity, CancellationToken ct = default);
}
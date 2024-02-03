namespace multi.lib.common;

public interface IClientRepository : IRepository<Client>
{
    Task<bool> Exists(string name, CancellationToken ct = default);
    Task<Client?> GetByAccessKey(string accessKey, CancellationToken ct = default);
}
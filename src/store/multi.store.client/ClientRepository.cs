using MongoDB.Driver;
using multi.lib.common;

namespace multi.store.client;
public class ClientRepository : IClientRepository
{
    private readonly IMongoCollection<Client> _clients;

    public ClientRepository(IMongoDatabase database)
    {
        _clients = database.GetCollection<Client>("Clients");
    }

    public async Task<bool> Upsert(Client entity, CancellationToken ct = default)
    {
        await _clients.InsertOneAsync(entity, new InsertOneOptions { }, ct);
        return true;
    }

    public async Task<Client?> Get(string id, CancellationToken ct = default)
    {
        var result = await _clients.Find(c => c.Id == id).FirstOrDefaultAsync(ct);
        return result;
    }

    public async Task<bool> Exists(string name, CancellationToken ct = default)
    {
        var result = await _clients.Find(c => c.Name == name).AnyAsync(ct);
        return result;
    }

    public async Task<IEnumerable<Client>> Get(CancellationToken ct = default)
    {
        var result = await _clients.AsQueryable().ToListAsync(ct) ?? new List<Client>();
        return result;
    }

    public async Task<Client?> GetByAccessKey(string accessKey, CancellationToken ct = default)
    {
        var result = await _clients.Find(c => c.AccessKey == accessKey).FirstOrDefaultAsync(ct);
        return result;
    }
}

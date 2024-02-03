using MongoDB.Driver;
using multi.lib.common;

namespace multi.store.shipment;

public class ShipmentRepository : IShipmentRepository
{
    private readonly IMongoCollection<Shipment> _shipments;

    public ShipmentRepository(IMongoDatabase database)
    {
        _shipments = database.GetCollection<Shipment>("Shipments");
    }

    public async Task<bool> Upsert(Shipment entity, CancellationToken ct = default)
    {
        var result = await _shipments.ReplaceOneAsync(c => c.Id == entity.Id, entity, new ReplaceOptions { IsUpsert = true }, ct);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public async Task<Shipment?> Get(string id, CancellationToken ct = default)
    {
        var result = await _shipments.Find(c => c.Id == id).FirstOrDefaultAsync(ct);
        return result;
    }

    public async Task<bool> Exists(string primaryReference, CancellationToken ct = default)
    {
        var result = await _shipments.Find(c => c.PrimaryReference == primaryReference).AnyAsync(ct);
        return result;
    }

    public async Task<IEnumerable<Shipment>> Get(CancellationToken ct = default)
    {
        var result = await _shipments.AsQueryable().ToListAsync(ct) ?? new List<Shipment>();
        return result;
    }
}

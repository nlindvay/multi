namespace multi.lib.common;

public interface IShipmentRepository : IRepository<Shipment>
{

    Task<bool> Exists(string primaryReference, CancellationToken ct = default);
}
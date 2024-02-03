namespace multi.lib.common.interfaces
{
    public interface ITmsPlugin
    {
        string GetPluginName();
        Task<Shipment> CreateShipmentAsync(Shipment shipment);
    }
}
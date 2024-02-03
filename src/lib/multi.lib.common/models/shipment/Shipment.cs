namespace multi.lib.common;

public class Shipment
{
    public string Id { get; set; }
    public string ClientId { get; set; }
    public string PrimaryReference { get; set; }
    public int Quantity { get; set; }
    public ShipmentStatus Status { get; set; }
}
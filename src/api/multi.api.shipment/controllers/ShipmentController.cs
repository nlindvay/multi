using Microsoft.AspNetCore.Mvc;
using multi.lib.common;
using multi.lib.common.interfaces;

namespace multi.api.shipment.Controllers;

[ApiController]
[Route("[controller]")]
public class ShipmentController : ControllerBase
{
    private readonly IShipmentRepository _shipmentRepository;
    private readonly ITmsPluginProvider _shipmentServiceProvider;
    private readonly ICurrentClientAccessor _currentClientAccessor;
    private readonly ILogger<ShipmentController> _logger;

    public ShipmentController(IShipmentRepository shipmentRepository, ITmsPluginProvider shipmentServiceProvider, ICurrentClientAccessor currentClientAccessor, ILogger<ShipmentController> logger)
    {
        _shipmentRepository = shipmentRepository;
        _shipmentServiceProvider = shipmentServiceProvider;
        _currentClientAccessor = currentClientAccessor;
        _logger = logger;
    }

    [HttpGet]
    [PermissionRequirements(Permission.ReadShipment)]
    public async Task<IEnumerable<Shipment>> Get(CancellationToken ct = default)
    {
        _logger.LogInformation("Getting shipments");
        var result = await _shipmentRepository.Get();
        return result;
    }

    [HttpPost]
    [PermissionRequirements(Permission.WriteShipment)]
    [FeatureRequirements(Feature.getRates, Feature.NewShipment)]
    public async Task<IActionResult> NewShipment(NewShipment newShipment, CancellationToken ct = default)
    {
        _logger.LogInformation("Creating new shipment");

        var shipment = new Shipment
        {
            Id = Guid.NewGuid().ToString(),
            ClientId = _currentClientAccessor.Get().Id,
            PrimaryReference = newShipment.PrimaryReference,
            Quantity = newShipment.Quantity,
            Status = ShipmentStatus.Created
        };
        
        var service = _shipmentServiceProvider.Get();
        
        if (service is null)
        {
            _logger.LogWarning("No service found for the current plugin");
            throw new Exception("No service found for the current plugin");
        }
        
        var tmsResult = await service.CreateShipmentAsync(shipment);
        var cacheResult = await _shipmentRepository.Upsert(shipment, ct);

        return Ok();
    }
}

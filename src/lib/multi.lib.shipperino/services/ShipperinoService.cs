using Microsoft.Extensions.Logging;
using multi.lib.common;
using multi.lib.common.interfaces;

namespace multi.lib.tms1
{
    public class ShipperinoService : ITmsPlugin
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ShipperinoService> _logger;

        public ShipperinoService(HttpClient httpClient, ILogger<ShipperinoService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public Task<Shipment> CreateShipmentAsync(Shipment shipment)
        {
            _logger.LogInformation("CreateShipmentAsync");
            return Task.FromResult(shipment);
        }

        public string GetPluginName() => "shipperino";

    }
}
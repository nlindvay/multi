using Microsoft.Extensions.Logging;
using multi.lib.common;
using multi.lib.common.interfaces;

namespace multi.lib.tms1
{
    public class ShipCoService : ITmsPlugin
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ShipCoService> _logger;

        public ShipCoService(HttpClient httpClient, ILogger<ShipCoService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public Task<Shipment> CreateShipmentAsync(Shipment shipment)
        {
            _logger.LogInformation("CreateShipmentAsync");
            return Task.FromResult(shipment);
        }

        public string GetPluginName() => "shipco";
       
    }
}
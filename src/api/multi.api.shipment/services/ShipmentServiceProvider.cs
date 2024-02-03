using multi.lib.common;
using multi.lib.common.interfaces;
using multi.lib.tms1;

namespace multi.api.shipment;

public class ShipmentServiceProvider : ITmsPluginProvider
{
    private readonly ILogger<ShipmentServiceProvider> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IServiceProvider _serviceProvider;

    public ShipmentServiceProvider(ILogger<ShipmentServiceProvider> logger, IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _serviceProvider = serviceProvider;
    }

    public ITmsPlugin? Get()
    {
        _logger.LogInformation("GetShipmentServiceProvider");

        var plugin = _httpContextAccessor?.HttpContext?.Items["CurrentPlugin"] as Plugin;

        if (plugin is null)
        {
            _logger.LogWarning("TMS Plugin not found in http context");
            throw new UnauthorizedAccessException("TMS Plugin not found in http context");
        }

        // get the current service collection
        var services = _serviceProvider.GetServices<ITmsPlugin>();
        return services.FirstOrDefault(s => s.GetPluginName() == plugin.Name);
    }
}

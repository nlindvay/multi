using multi.lib.common;

namespace multi.api.shipment;

public class CurrentClientAccessor : ICurrentClientAccessor
{
    private readonly ILogger<CurrentClientAccessor> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentClientAccessor(ILogger<CurrentClientAccessor> logger, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public Client Get()
    {
        _logger.LogInformation("GetCurrentClient executing");

        var client = _httpContextAccessor!.HttpContext!.Items["CurrentClient"] as Client;
        
        if (client is null)
        {
            _logger.LogWarning("Client not found in http context");
            throw new UnauthorizedAccessException("Client not found in http context");
        }
        return client;
    }
}

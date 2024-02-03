using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using multi.lib.common;

namespace multi.api.shipment.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientController : ControllerBase
{
    private readonly IClientRepository _clientRepository;
    private readonly ILogger<ClientController> _logger;
    private readonly TmsOptions _tmsOptions;

    public ClientController(IClientRepository clientRepository, ILogger<ClientController> logger, IOptions<TmsOptions> tmsOptions)
    {
        _clientRepository = clientRepository;
        _logger = logger;
        _tmsOptions = tmsOptions.Value;
    }

    [HttpGet]
    [NoAuthorization]
    public async Task<IEnumerable<Client>> Get(CancellationToken ct = default)
    {
        _logger.LogInformation("Getting clients");
        var result = await _clientRepository.Get(ct);
        return result;
    }

    [HttpPost]
    [NoAuthorization]
    public async Task<IActionResult> NewClientConfiguration(NewClient newClient, CancellationToken ct = default)
    {
        _logger.LogInformation("Creating new client");

        if (newClient.Permissions is null || newClient.Permissions.Length == 0)
        {
            return BadRequest("Permissions are required.");
        }

        if (string.IsNullOrWhiteSpace(newClient.Name))
        {
            return BadRequest("Name is required.");
        }

        if (string.IsNullOrWhiteSpace(newClient.PluginId) || !_tmsOptions.Plugins.Any(p => p.Id == newClient.PluginId))
        {
            return BadRequest("PluginId is required.");
        }

        var client = new Client
        {
            Id = Guid.NewGuid().ToString(),
            Name = newClient.Name,
            AccessKey = Guid.NewGuid().ToString(),
            Permissions = newClient.Permissions,
            IsAdmin = newClient.IsAdmin,
            PluginId = newClient.PluginId
        };

        if (await _clientRepository.Exists(client.Name, ct))
        {
            return BadRequest("Client already exists.");
        }

        var cacheResult = await _clientRepository.Upsert(client, ct);

        return cacheResult ? Ok() : BadRequest();
    }
}

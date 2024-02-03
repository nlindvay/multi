using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using multi.lib.common;
using multi.lib.common.interfaces;
using multi.lib.tms1;

namespace multi.api.shipment;

public class AuthorizationFilter : IAsyncActionFilter
{
    private readonly ILogger<AuthorizationFilter> _logger;
    private readonly TmsOptions _tmsOptions;
    private readonly IClientRepository _clientRepository;

    public AuthorizationFilter(ILogger<AuthorizationFilter> logger, IOptions<TmsOptions> tmsOptions, IClientRepository clientRepository)
    {
        _logger = logger;
        _tmsOptions = tmsOptions.Value;
        _clientRepository = clientRepository;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.ActionDescriptor.EndpointMetadata.Any(em => em.GetType() == typeof(NoAuthorizationAttribute)))
        {
            await next();
            return;
        }

        _logger.LogInformation("AuthorizationFilter executing");

        if (!context.HttpContext.Request.Headers.ContainsKey("AccessKey"))
        {
            _logger.LogWarning("AccessKey not found in request headers");
            context.Result = new UnauthorizedResult();
            return;
        }

        var accessKey = context.HttpContext.Request.Headers["AccessKey"];
        
        var client = await _clientRepository.GetByAccessKey(context.HttpContext.Request.Headers["AccessKey"]);

        if (client is null)
        {
            _logger.LogWarning("Client not found for the provided access key");
            context.Result = new UnauthorizedResult();
            return;
        }
        else
        {
            context.HttpContext.Items.Add("CurrentClient", client);
        }

        var featureRequirements = context.ActionDescriptor.EndpointMetadata
            .OfType<FeatureRequirementsAttribute>()
            .FirstOrDefault();

        if (featureRequirements is not null)
        {
            var plugin = _tmsOptions.Plugins.FirstOrDefault(p => p.Id == client.PluginId);


            if (plugin is null)
            {
                _logger.LogWarning("A TMS plugin is not configured for your client");
                context.Result = new UnauthorizedResult();
                return;
            }

            if (!plugin.HasFeatures(featureRequirements.Features))
            {
                _logger.LogWarning("The configured TMS plugin does not have required features for this endpoint");
                context.Result = new UnauthorizedResult();
                return;
            }

            context.HttpContext.Items.Add("CurrentPlugin", plugin);
        }

        var permissionRequirements = context.ActionDescriptor.EndpointMetadata
            .OfType<PermissionRequirementsAttribute>()
            .FirstOrDefault();

        if (permissionRequirements is not null)
        {
            if (!client.HasPermissions(permissionRequirements.Permissions))
            {
                _logger.LogWarning("Client does not have required permissions for this endpoint");
                context.Result = new UnauthorizedResult();
                return;
            }
        }

        await next();
    }
}
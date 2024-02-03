namespace multi.lib.common;

public class Client
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string AccessKey { get; set; }
    public string[] Permissions { get; set; }
    public bool IsAdmin { get; set; }
    public string PluginId { get; set; }

    public bool HasPermission(string permission) => Permissions.Contains(permission);
    public bool HasPermissions(string[] permissions) => permissions.All(p => Permissions.Contains(p));
}
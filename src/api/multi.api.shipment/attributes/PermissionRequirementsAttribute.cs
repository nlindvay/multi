namespace multi.api.shipment;

public class PermissionRequirementsAttribute : Attribute
{
    public string[] Permissions { get; }

    public PermissionRequirementsAttribute(params string[] permissions)
    {
        Permissions = permissions;
    }
}


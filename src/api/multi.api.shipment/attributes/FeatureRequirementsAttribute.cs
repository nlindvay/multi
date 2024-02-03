namespace multi.api.shipment;

public class FeatureRequirementsAttribute : Attribute
{
    public string[] Features { get; }

    public FeatureRequirementsAttribute(params string[] features)
    {
        Features = features;
    }
}


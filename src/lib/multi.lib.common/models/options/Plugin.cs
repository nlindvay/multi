namespace multi.lib.common;

public class Plugin
{
    public string Id { get; set; }
    public string Name { get; set; }
    public bool Enabled { get; set; }
    public string[] Features { get; set; }

    public bool HasFeature(string feature) => Features.Contains(feature);
    public bool HasFeatures(string[] feature) => feature.All(f => Features.Contains(f));
}
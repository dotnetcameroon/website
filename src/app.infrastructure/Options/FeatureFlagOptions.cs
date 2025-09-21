namespace app.infrastructure.Options;

public class FeatureFlagOptions
{
    public const string SectionName = "FeatureFlags";
    public Dictionary<string, FeatureFlag> Features { get; set; } = [];
    public bool EnableDevModeOverride { get; set; } = true;
}

public class FeatureFlag
{
    public bool Enabled { get; set; }
    public bool EnabledInProduction { get; set; }
    public bool EnabledInDevMode { get; set; } = true;
    public string? Description { get; set; }
    public Dictionary<string, object>? Properties { get; set; }
}

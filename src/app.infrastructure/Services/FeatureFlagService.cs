using app.business.Services;
using app.infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace app.infrastructure.Services;

public class FeatureFlagService : IFeatureFlagService
{
    private readonly FeatureFlagOptions _options;
    private readonly IDevModeService _devModeService;
    private readonly IHostEnvironment _environment;
    private readonly IConfiguration _configuration;

    public FeatureFlagService(
        IOptions<FeatureFlagOptions> options,
        IDevModeService devModeService,
        IHostEnvironment environment,
        IConfiguration configuration)
    {
        _options = options.Value;
        _devModeService = devModeService;
        _environment = environment;
        _configuration = configuration;
    }

    public Task<bool> IsEnabledAsync(string featureName)
    {
        if (!_options.Features.TryGetValue(featureName, out var feature))
        {
            // Feature not found, default to false
            return Task.FromResult(false);
        }

        bool isEnabled;

        // Check environment-specific settings
        if (_environment.IsProduction())
        {
            isEnabled = feature.EnabledInProduction;
        }
        else if (_devModeService.IsDevMode && _options.EnableDevModeOverride)
        {
            // In Dev Mode, use dev mode specific flag
            isEnabled = feature.EnabledInDevMode;
        }
        else
        {
            // Default to the general enabled flag
            isEnabled = feature.Enabled;
        }

        return Task.FromResult(isEnabled);
    }

    public Task<T> GetFeatureValueAsync<T>(string featureName, T defaultValue)
    {
        if (!_options.Features.TryGetValue(featureName, out var feature) || feature.Properties == null)
        {
            return Task.FromResult(defaultValue);
        }

        // First check if the feature is enabled
        var isEnabledTask = IsEnabledAsync(featureName);
        if (!isEnabledTask.Result)
        {
            return Task.FromResult(defaultValue);
        }

        // Try to get the value from properties
        if (feature.Properties.TryGetValue("value", out var value))
        {
            try
            {
                if (value is T typedValue)
                {
                    return Task.FromResult(typedValue);
                }

                // Try to convert the value
                var convertedValue = (T)Convert.ChangeType(value, typeof(T));
                return Task.FromResult(convertedValue);
            }
            catch
            {
                // If conversion fails, return default value
                return Task.FromResult(defaultValue);
            }
        }

        return Task.FromResult(defaultValue);
    }

    public async Task<Dictionary<string, bool>> GetAllFeaturesAsync()
    {
        var features = new Dictionary<string, bool>();

        foreach (var feature in _options.Features)
        {
            features[feature.Key] = await IsEnabledAsync(feature.Key);
        }

        return features;
    }
}
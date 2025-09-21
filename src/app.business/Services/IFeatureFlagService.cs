namespace app.business.Services;

public interface IFeatureFlagService
{
    Task<bool> IsEnabledAsync(string featureName);
    Task<T> GetFeatureValueAsync<T>(string featureName, T defaultValue);
    Task<Dictionary<string, bool>> GetAllFeaturesAsync();
}
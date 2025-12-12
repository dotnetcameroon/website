using app.business.Services;
using app.infrastructure.Services;

namespace app.Extensions;

public static class CacheManagerExtensions
{
    public static IServiceCollection AddCacheManager(this IServiceCollection services)
    {
        services.AddMemoryCache(opt => opt.ExpirationScanFrequency = TimeSpan.FromDays(1));
        services.AddSingleton<ICacheManager, CacheManager>();
        return services;
    }
}

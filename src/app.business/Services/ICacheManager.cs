using Microsoft.Extensions.Caching.Memory;

namespace app.business.Services;

public interface ICacheManager
{

    /// <summary>
    /// Gets the list of keys in the cache.
    /// </summary>
    /// <returns>The list of keys in the cache.</returns>
    string[] GetKeys();

    /// <summary>
    /// Gets the item associated with this key if present.
    /// </summary>
    /// <param name="key">An object identifying the requested entry.</param>
    /// <param name="value">The located value or null.</param>
    /// <returns>True if the key was found.</returns>
    bool TryGetValue(string key, out object? value);

    /// <summary>
    /// Create or overwrite an entry in the cache.
    /// </summary>
    /// <param name="key">An object identifying the entry.</param>
    /// <returns>The newly created <see cref="ICacheEntry"/> instance.</returns>
    ICacheEntry CreateEntry(string key);

    /// <summary>
    /// Removes the object associated with the given key.
    /// </summary>
    /// <param name="key">An object identifying the entry.</param>
    void Remove(string key);
    Task<TItem?> GetOrCreateAsync<TItem>(string key, Func<ICacheEntry, Task<TItem>> factory);

#if NET6_0_OR_GREATER
    /// <summary>
    /// Gets a snapshot of the cache statistics if available.
    /// </summary>
    /// <returns>An instance of <see cref="MemoryCacheStatistics"/> containing a snapshot of the cache statistics.</returns>
    MemoryCacheStatistics? GetCurrentStatistics() => null;
#endif
}

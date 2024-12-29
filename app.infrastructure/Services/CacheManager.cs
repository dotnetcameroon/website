using System.Collections.Concurrent;
using app.business.Services;
using Microsoft.Extensions.Caching.Memory;

namespace app.infrastructure.Services;
public sealed class CacheManager(IMemoryCache memoryCache) : ICacheManager
{
    private readonly IMemoryCache _memoryCache = memoryCache;
    private readonly ConcurrentDictionary<string, byte> _keys = [];

    public ICacheEntry CreateEntry(string key)
    {
        _keys.TryAdd(key, 0);
        return _memoryCache.CreateEntry(key);
    }

    public string[] GetKeys()
    {
        return [.. _keys.Keys];
    }

    public Task<TItem?> GetOrCreateAsync<TItem>(string key, Func<ICacheEntry, Task<TItem>> factory)
    {
        _keys.TryAdd(key, 0);
        return _memoryCache.GetOrCreateAsync(key, factory);
    }

    public void Dispose()
    {
        _keys.Clear();
        _memoryCache.Dispose();
    }

    public void Remove(string key)
    {
        _memoryCache.Remove(key);
        _keys.TryRemove(key, out _);
    }

    public bool TryGetValue(string key, out object? value)
    {
        return _memoryCache.TryGetValue(key, out value);
    }
}

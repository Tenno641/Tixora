using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Tixora.Shared.Application.Common;

namespace Tixora.Shared.Infrastructure.Common;

public class CacheService: ICacheService
{
    private readonly IDistributedCache _cache;
    
    public CacheService(IDistributedCache cache)
    {
        _cache = cache;
    }
    
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        byte[]? result = await _cache.GetAsync(key, cancellationToken);
        
        return result == null
            ? default
            : JsonSerializer.Deserialize<T>(result);
    }
    
    public async Task SetAsync<T>(string key, T item, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        byte[] value = JsonSerializer.SerializeToUtf8Bytes(item);
        await _cache.SetAsync(key, value, CacheOptions.Create(expiration), cancellationToken);
    }
    
    public async Task DeleteAsync(string key, CancellationToken cancellationToken = default)
    {
        await _cache.RemoveAsync(key, cancellationToken);
    }
}

public static class CacheOptions
{
    public static DistributedCacheEntryOptions Create(TimeSpan? slidingExpiration) => new DistributedCacheEntryOptions
    {
        AbsoluteExpirationRelativeToNow = slidingExpiration ?? TimeSpan.FromHours(24)
    };
}
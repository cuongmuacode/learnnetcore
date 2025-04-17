using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Internal;

namespace LearnNetCore.Application.Cache;

public class MemoryCacheService : ICacheService
{
    private readonly MemoryCache _memoryCache;

    public MemoryCacheService()
    {
        var options = new MemoryCacheOptions
        {
            SizeLimit = 1024 * 1024 * 100, // 100 MB,
            Clock = new SystemClock(),
            CompactionPercentage = 0.2f,
            ExpirationScanFrequency = TimeSpan.FromMinutes(5),
            TrackLinkedCacheEntries = true,
            TrackStatistics = true
        };
        _memoryCache = new MemoryCache(options);

    }

    public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> findAsync)
    {
        var options = new MemoryCacheEntryOptions
        {
            Size = 1, // Phải có nếu bạn dùng SizeLimit
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10), // Cache sẽ hết hạn sau 10 phút
            SlidingExpiration = TimeSpan.FromMinutes(5), // Nếu không được truy cập trong 5 phút thì hết hạn
            Priority = CacheItemPriority.Normal // Ưu tiên xóa khi cần giải phóng bộ nhớ
        };
        if (_memoryCache.TryGetValue(key, out T value) && value != null)
        {
            return value;
        }
        var result = await findAsync();
        if (result == null) return default;
        _memoryCache.Set(key, result, options);
        return result;
    }

    public void Remove(string key)
    {
        _memoryCache.Remove(key);
    }
}

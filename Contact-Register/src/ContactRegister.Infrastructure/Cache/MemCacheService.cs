using ContactRegister.Application.Interfaces.Services;
using Microsoft.Extensions.Caching.Memory;

namespace ContactRegister.Infrastructure.Cache
{
    public class MemCacheService(IMemoryCache cache): ICacheService
    {
        private readonly IMemoryCache _cache = cache;

        public object? Get(string key) => _cache.TryGetValue(key, out var cachedValue) ? cachedValue : null;
        
        public void Remove(string key) => _cache.Remove(key);

        public void Set(string key, object value) => _cache.Set(key, value, TimeSpan.FromMinutes(10));
        
        public async Task<T?> GetOrCreateAsync<T>(string key, Func<Task<T>> func)
        {
            // Attempt to get the item from cache
            if (_cache.TryGetValue(key, out T? cachedValue))
            {
                return cachedValue!;
            }

            // If it doesn't exist, create it asynchronously
            T newValue = await func();

            if (newValue == null)
                return default;

            // Set up sliding expiration (e.g., 5 minutes)
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(5)
            };

            // Store item in cache
            _cache.Set(key, newValue, cacheEntryOptions);

            return newValue;
        }
    }
}

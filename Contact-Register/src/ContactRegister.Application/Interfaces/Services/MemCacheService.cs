using Microsoft.Extensions.Caching.Memory;

namespace ContactRegister.Application.Interfaces.Services
{
    public class MemCacheService(IMemoryCache cache): ICacheService
    {
        private readonly IMemoryCache _cache = cache;

        public object? Get(string key) => _cache.TryGetValue(key, out var cachedValue) ? cachedValue : null;
        
        public void Remove(string key) => _cache.Remove(key);

        public void Set(string key, object value) => _cache.Set(key, value, TimeSpan.FromMinutes(10));
    }
}

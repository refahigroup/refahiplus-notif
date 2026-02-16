using Microsoft.Extensions.Caching.Memory;
using Refahi.Notif.Domain.Contract;

namespace Refahi.Notif.Infrastructure.MemoryCache
{
    public class InMemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;


        public InMemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }


        public async Task<T?> GetAsync<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }
        public async Task SetAsync<T>(string key, T value, DateTimeOffset absoluteExpiration)
        {
            _memoryCache.Set(key, value, absoluteExpiration);
        }
    }
}

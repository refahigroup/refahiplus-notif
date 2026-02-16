using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Refahi.Notif.Domain.Contract;
using System.Runtime.CompilerServices;

namespace Refahi.Notif.Infrastructure.RedisCache
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;


        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }


        public async Task<T?> GetAsync<T>(string key)
        {
            var json = await _cache.GetStringAsync(key);

            if (string.IsNullOrEmpty(json))
                return default(T);

            return JsonConvert.DeserializeObject<T>(json);
        }
        public async Task SetAsync<T>(string key, T value, DateTimeOffset absoluteExpiration)
        {
            var json = JsonConvert.SerializeObject(value);
            _cache.SetString(key, json, new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = absoluteExpiration
            });
        }
    }
}

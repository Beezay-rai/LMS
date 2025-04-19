using LMS.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace LMS.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _redisDb;
        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _redisDb = _connectionMultiplexer.GetDatabase();
        }
        public async Task SetCacheValueAsync(string key, string value, TimeSpan timeSpan =default)
        {
            if (timeSpan == default)
            {
                timeSpan = TimeSpan.FromSeconds(10);
            }
            await _redisDb.StringSetAsync(key, value, timeSpan);
        }

        public async Task<string> GetCacheValueAsync(string key)
        {
            return await _redisDb.StringGetAsync(key);
        }

        public async Task DeleteCacheKey(string key)
        {
            await _redisDb.KeyDeleteAsync(key);
        }

        

    }
}

using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace LMS.Cache
{
    public class RedisCache : IRedisCache
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;
        private readonly ConfigurationOptions _redisConfig;
        private readonly IConfiguration _config;

        public RedisCache(IConfiguration config)
        {
            _config = config;
            _redisConfig = new ConfigurationOptions
            {
                EndPoints = { _config.GetConnectionString("Redis") },
                AbortOnConnectFail = false,
                AllowAdmin = true,
                ConnectTimeout = 5000,
                SyncTimeout = 5000,
                // ResponseTimeout = 5000
            };
            _connectionMultiplexer = ConnectionMultiplexer.Connect(_redisConfig);
        }

        public void HealthCheck()
        {
            var db = _connectionMultiplexer.GetDatabase();
            db.Ping();
        }
    }
}

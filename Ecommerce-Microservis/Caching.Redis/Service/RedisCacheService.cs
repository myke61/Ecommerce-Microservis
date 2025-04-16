using Caching.Redis.Interface;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Caching.Redis.Service
{
    public class RedisCacheService(IConnectionMultiplexer connectionMultiplexer) : IRedisCache
    {
        private readonly IDatabase _database = connectionMultiplexer.GetDatabase();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cacheItem"></param>
        /// <param name="expirationTime"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task AddAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var redisKey = key.ToString() ?? throw new ArgumentException("Key cannot be null or empty.", nameof(key));
            var serializedValue = JsonSerializer.Serialize(value);

            await _database.StringSetAsync(redisKey, serializedValue, expiry);
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var redisKey = key.ToString() ?? throw new ArgumentException("Key cannot be null or empty.", nameof(key));
            var value = await _database.StringGetAsync(key);

            return value.HasValue ? JsonSerializer.Deserialize<T>(value) : default;
        }

        public void RemoveAsync(string key)
        {
            var redisKey = key.ToString() ?? throw new ArgumentException("Key cannot be null or empty.", nameof(key));
            _database.KeyDelete(redisKey);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caching.Redis.Interface
{
    public interface IRedisCache
    {
        Task<T?> GetAsync<T>(string key);
        Task AddAsync<T>(string key, T value, TimeSpan? expiry = null);
        void RemoveAsync(string key);
    }
}

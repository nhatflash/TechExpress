using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;
using TechExpress.Repository.CustomExceptions;

namespace TechExpress.Service.Utils
{
    public class RedisUtils
    {
        private readonly IDistributedCache _redisCache;

        public RedisUtils(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }

        public async Task StoreStringData(string key, string data, TimeSpan expiration)
        {
            await _redisCache.SetStringAsync(key, data, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            });
        }

        public async Task StoreLongData(string key, long data, TimeSpan expiration)
        {
            string dataStr = data.ToString();
            await StoreStringData(key, dataStr, expiration);
        }


        public async Task StoreGuidData(string key, Guid data, TimeSpan expiration)
        {
            string dataStr = data.ToString();
            await StoreStringData(key, dataStr, expiration);
        }


        public async Task<string?> GetStringDataFromKey(string key)
        {
            return await _redisCache.GetStringAsync(key);
        }

    }
}

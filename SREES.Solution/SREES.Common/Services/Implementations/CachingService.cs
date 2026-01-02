using System.Collections.Concurrent;
using System.Runtime.Caching;
using SREES.Common.Services.Interfaces;

namespace SREES.Common.Services.Implementations
{
    public class CachingService : ICachingService
    {
        // Concurrent set to track all cache keys
        private static readonly ConcurrentDictionary<string, byte> _cacheKeys = new ConcurrentDictionary<string, byte>();

        public Task<T?> GetObjectsFromCache<T>(string cacheKey) 
        {
            ObjectCache cache = MemoryCache.Default;
            var cachedObjects = (T?)cache.Get(cacheKey);
            if (EqualityComparer<T>.Default.Equals(cachedObjects, default)) 
            {
                throw new KeyNotFoundException($"No objects found in cache for key: {cacheKey}");
            }
            return Task.FromResult(cachedObjects);
        }

        public Task<bool> ObjectsCached<T>(string cacheKey)
        {
            ObjectCache cache = MemoryCache.Default;
            var cachedObjects = (T?)cache.Get(cacheKey);
            if (EqualityComparer<T>.Default.Equals(cachedObjects, default))
                return Task.FromResult(false);
            
            return Task.FromResult(true);
        }

        public Task RemoveObjectsFromCache(string cacheKey)
        {
            ObjectCache cache = MemoryCache.Default;
            cache.Remove(cacheKey);
            _cacheKeys.TryRemove(cacheKey, out _);

            return Task.CompletedTask;
        }

        public Task RemoveObjectsFromCacheByPattern(string pattern)
        {
            ObjectCache cache = MemoryCache.Default;
            
            // Find all keys that match the pattern
            var keysToRemove = _cacheKeys.Keys
                .Where(key => key.StartsWith(pattern, StringComparison.OrdinalIgnoreCase))
                .ToList();

            // Remove each matching key
            foreach (var key in keysToRemove)
            {
                cache.Remove(key);
                _cacheKeys.TryRemove(key, out _);
            }

            return Task.CompletedTask;
        }

        public Task SetObjectsInCache<T>(string cacheKey, T data, int expirationTime)
        {
            ObjectCache cache = MemoryCache.Default;
            if (!cache.Contains(cacheKey)) 
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                if (expirationTime == 0) 
                {
                    policy.AbsoluteExpiration = DateTimeOffset.MaxValue;
                }
                else 
                {
                    policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(expirationTime);
                }

                // Add callback to remove key from tracking when cache expires
                policy.RemovedCallback = arguments =>
                {
                    _cacheKeys.TryRemove(cacheKey, out _);
                };

                var cachedObjects = data;
                cache.Set(cacheKey, cachedObjects, policy);
                
                // Track the cache key
                _cacheKeys.TryAdd(cacheKey, 0);
            }

            return Task.CompletedTask;
        }

        public Task<bool> UpdateObjectsInCache<T>(string cacheKey, T data, int expirationTime)
        {
            ObjectCache cache = MemoryCache.Default;
            var cachedObjects = (T?)cache.Get(cacheKey);
            if (EqualityComparer<T>.Default.Equals(cachedObjects, default))
                return Task.FromResult(false);

            cache.Remove(cacheKey);
            _cacheKeys.TryRemove(cacheKey, out _);
            SetObjectsInCache(cacheKey, data, expirationTime);
            
            return Task.FromResult(true);
        }
    }
}

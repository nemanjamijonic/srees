namespace SREES.Common.Services.Interfaces
{
    public interface ICachingService
    {
        Task<T?> GetObjectsFromCache<T>(string cacheKey);
        Task SetObjectsInCache<T>(string cacheKey, T data, int expirationTime);
        Task<bool> ObjectsCached<T>(string cacheKey);
        Task<bool> UpdateObjectsInCache<T>(string cacheKey, T data, int expirationTime);
        Task RemoveObjectsFromCache(string cacheKey);
        Task RemoveObjectsFromCacheByPattern(string pattern);
    }
}

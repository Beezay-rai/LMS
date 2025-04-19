namespace LMS.Interfaces
{
    public interface ICacheService
    {
        Task SetCacheValueAsync(string key, string value, TimeSpan timeSpan = default);
        Task<string> GetCacheValueAsync(string key);
        Task DeleteCacheKey(string key);
    }
}

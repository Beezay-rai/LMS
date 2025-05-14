using LMS.Interfaces;
using LMS.Models.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using System.Text.Json;

namespace LMS.Filters
{
    public class CacheFilter : Attribute, IFilterFactory
    {
        public bool IsReusable => false;
        private readonly int _timeToLiveInSeconds;

        public CacheFilter(int timeToLiveInSeconds = 5)
        {
            _timeToLiveInSeconds = timeToLiveInSeconds;
        }

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var cacheService = serviceProvider.GetRequiredService<ICacheService>();
            var cacheSetting = serviceProvider.GetRequiredService<RedisCacheSetting>();

            return new CacheFilterImpl(_timeToLiveInSeconds, cacheService, cacheSetting);
        }

        private class CacheFilterImpl : IAsyncActionFilter
        {
            private readonly int _timeToLiveInSeconds;
            private readonly ICacheService _cacheService;
            private readonly RedisCacheSetting _cacheSetting;

            public CacheFilterImpl(int ttl, ICacheService cacheService, RedisCacheSetting cacheSetting)
            {
                _timeToLiveInSeconds = ttl;
                _cacheService = cacheService;
                _cacheSetting = cacheSetting;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (_cacheSetting is null || !_cacheSetting.Enable || context.HttpContext.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
                {
                    await next();
                    return;
                }

                var request = context.HttpContext.Request;
                var cacheKey = GenerateCacheKey(request);
                var settings = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = new LowercaseNamingPolicy()
                };

                var cachedResponse = await _cacheService.GetCacheValueAsync(cacheKey);
                if (!string.IsNullOrEmpty(cachedResponse))
                {
                    var apiResponse = JsonSerializer.Deserialize<object>(cachedResponse, settings);
                    context.Result = new ObjectResult(apiResponse);
                    return;
                }

                var executedContext = await next();
                int statusCode = executedContext.HttpContext.Response.StatusCode;
                if (statusCode >= 200 && statusCode < 300 && executedContext.Result is ObjectResult objectResult)
                {
                    var serialized = JsonSerializer.Serialize(objectResult.Value, settings);
                    await _cacheService.SetCacheValueAsync(cacheKey, serialized, TimeSpan.FromSeconds(_timeToLiveInSeconds));
                }
            }

            private static string GenerateCacheKey(HttpRequest request)
            {
                var keyBuilder = new StringBuilder();
                keyBuilder.Append($"{request.Path}");
                foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
                {
                    keyBuilder.Append($"|{key}-{value}");
                }
                return keyBuilder.ToString();
            }
        }

        public class LowercaseNamingPolicy : JsonNamingPolicy
        {
            public override string ConvertName(string name) => name.ToLower();
        }
    }
}

using LMS.Interfaces;
using LMS.Models.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using System.Text.Json;

namespace LMS.Filters
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveSeconds;
        public CachedAttribute(int timeToLiveSeconds)
        {
            _timeToLiveSeconds = timeToLiveSeconds;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var request = context.HttpContext.Request;
            var cacheKey = GenerateCacheKey(request);
            var cacheSettings = context.HttpContext.RequestServices.GetRequiredService<RedisCacheSetting>();

            if (cacheSettings is null || !cacheSettings.Enable || !context.HttpContext.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                await next();
                return;
            }


            var settings = new JsonSerializerOptions
            {
                PropertyNamingPolicy = new LowercaseNamingPolicy()
            };
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

            var cachedResponse = await cacheService.GetCacheValueAsync(cacheKey);
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
                await cacheService.SetCacheValueAsync(cacheKey, serialized, TimeSpan.FromSeconds(_timeToLiveSeconds));
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
        public class LowercaseNamingPolicy : JsonNamingPolicy
        {
            public override string ConvertName(string name) => name.ToLower();
        }

    }
}

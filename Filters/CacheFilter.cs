using LMS.Cache;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LMS.Filters
{
    public class CacheFilter:ActionFilterAttribute
    {
        private readonly int _timeToLive;
        public CacheFilter(int timeToLive)
        {
            _timeToLive = timeToLive;
        }
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cache = context.HttpContext.RequestServices.GetService<RedisCache>();

            return base.OnActionExecutionAsync(context, next);
        }
    }
}

using Microsoft.Extensions.Configuration;

namespace LMS.Cache
{
    public interface IRedisCache
    {
        void HealthCheck();
    }
}

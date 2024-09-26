using IForgotMyPassword.Abstraction;
using Microsoft.Extensions.Caching.Memory;

namespace IForgotMyPassword.Concrete
{
    public class CacheService : ICacheService
    {
        readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public string GetCache(string key)
        {
            string code = _memoryCache.Get<string>(key);
            return code;
        }

        public void SetCache(string key, string value)
        {
            _memoryCache.Set(key, value, TimeSpan.FromSeconds(60));
        }
    }
}

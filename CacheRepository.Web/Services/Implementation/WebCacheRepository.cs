using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Caching;
using CacheRepository.Implementation;

namespace CacheRepository.Web.Services.Implementation
{
    public class WebCacheRepository : CacheRepositoryBase
    {
        private readonly Cache _cache;
        
        public WebCacheRepository()
        {
            _cache = HttpContext.Current == null
                ? HttpRuntime.Cache
                : HttpContext.Current.Cache;
        }

        protected override void Set(string key, object value, DateTime? expiration, TimeSpan? sliding)
        {
            if (sliding.HasValue)
                _cache.Insert(key, value, null, Cache.NoAbsoluteExpiration, sliding.Value);
            else if (expiration.HasValue)
                _cache.Insert(key, value, null, expiration.Value, Cache.NoSlidingExpiration);
            else
                _cache.Insert(key, value);
        }

        public override object Get(string key)
        {
            return _cache.Get(key);
        }

        public override void Remove(string key)
        {
            _cache.Remove(key);
        }

        public override void ClearAll()
        {
            var keys = _cache
                .Cast<DictionaryEntry>()
                .Select(entry => entry.Key.ToString())
                .ToList();

            foreach (var key in keys)
                _cache.Remove(key);
        }
    }
}

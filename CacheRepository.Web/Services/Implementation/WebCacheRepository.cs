using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using CacheRepository.Configuration;
using CacheRepository.Implementation.Base;

namespace CacheRepository.Web.Services.Implementation
{
    public class WebCacheRepository : AsyncCacheRepositoryBase
    {
        private readonly Cache _cache;

        public WebCacheRepository(ICacheSettings cacheSettings)
            : base(cacheSettings)
        {
            _cache = HttpContext.Current == null
                ? HttpRuntime.Cache
                : HttpContext.Current.Cache;
        }

        public override Task RemoveAsync(string key, CancellationToken cancelToken)
        {
            _cache.Remove(key);

            return Task.FromResult(true);
        }

        public override Task ClearAllAsync(CancellationToken cancelToken)
        {
            var keys = _cache
                .Cast<DictionaryEntry>()
                .Select(entry => entry.Key.ToString())
                .ToArray();

            foreach (var key in keys)
                _cache.Remove(key);

            return Task.FromResult(true);
        }

        protected override Task<Tuple<bool, T>> TryGetAsync<T>(string key, CancellationToken cancelToken)
        {
            var value = _cache.Get(key);
            var result = Tuple.Create(value == null, (T)value);
            return Task.FromResult(result);
        }

        protected override Task SetAsync<T>(string key, T value, DateTime? expiration, TimeSpan? sliding, CancellationToken cancelToken)
        {
            if (sliding.HasValue)
                _cache.Insert(key, value, null, Cache.NoAbsoluteExpiration, sliding.Value);
            else if (expiration.HasValue)
                _cache.Insert(key, value, null, expiration.Value, Cache.NoSlidingExpiration);
            else
                _cache.Insert(key, value);

            return Task.FromResult(true);
        }
    }
}

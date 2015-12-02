using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using CacheRepository.Configuration.Implementation;
using CacheRepository.Implementation.Base;

namespace CacheRepository.Tests.Utilities
{
    public class TestMemoryCacheRepository : AsyncCacheRepositoryBase
    {
        private readonly ConcurrentDictionary<string, dynamic> _map = new ConcurrentDictionary<string, dynamic>();

        public TestMemoryCacheRepository()
            : base(ConfigurationCacheSettings.Instance)
        {
        }

        public ConcurrentDictionary<string, object> Map
        {
            get { return _map; }
        }

        public DateTime? LastExpiration { get; private set; }

        public TimeSpan? LastSliding { get; private set; }

        public override Task RemoveAsync(string key, CancellationToken cancelToken)
        {
            object value;
            _map.TryRemove(key, out value);

            return Task.FromResult(true);
        }

        public override Task ClearAllAsync(CancellationToken cancelToken)
        {
            _map.Clear();

            return Task.FromResult(true);
        }

        protected override Task<Tuple<bool, T>> TryGetAsync<T>(string key, CancellationToken cancelToken)
        {
            dynamic value;
            var result = _map.TryGetValue(key, out value)
                ? Tuple.Create(true, (T) value)
                : Tuple.Create(false, default(T));

            return Task.FromResult(result);
        }

        protected override Task SetAsync<T>(string key, T value, DateTime? expiration, TimeSpan? sliding, CancellationToken cancelToken)
        {
            LastExpiration = expiration;
            LastSliding = sliding;

            _map[key] = value;

            return Task.FromResult(true);
        }
    }
}
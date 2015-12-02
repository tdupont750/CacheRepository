using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using CacheRepository.Configuration;
using CacheRepository.Implementation.Base;

namespace CacheRepository.Implementation
{
    public class InMemoryCacheRepository : AsyncCacheRepositoryBase
    {
        private readonly ConcurrentDictionary<string, Tuple<DateTime?, TimeSpan?, object>> _map
            = new ConcurrentDictionary<string, Tuple<DateTime?, TimeSpan?, object>>();

        public InMemoryCacheRepository(ICacheSettings cacheSettings)
            : base(cacheSettings)
        {
        }

        public override Task RemoveAsync(string key, CancellationToken cancelToken)
        {
            Tuple<DateTime?, TimeSpan?, dynamic> value;
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
            Tuple<DateTime?, TimeSpan?, object> value;
            if (!_map.TryGetValue(key, out value))
                return Task.FromResult(Tuple.Create(false, default(T)));

            if (value.Item1.HasValue && value.Item1.Value < DateTime.UtcNow)
            {
                _map.TryRemove(key, out value);
                return Task.FromResult(Tuple.Create(false, default(T)));
            }

            if (value.Item2.HasValue)
                SetAsync(key, value.Item3, value.Item2.Value, cancelToken);

            return Task.FromResult(Tuple.Create(true, (T)value.Item3));
        }

        protected override Task SetAsync<T>(string key, T value, DateTime? expiration, TimeSpan? sliding, CancellationToken cancelToken)
        {
            if (sliding.HasValue)
                expiration = DateTime.UtcNow.Add(sliding.Value);

            _map[key] = Tuple.Create(expiration, sliding, (object)value);

            return Task.FromResult(true);
        }
    }
}
using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CacheRepository.Configuration;
using CacheRepository.Configuration.Implementation;
using CacheRepository.Implementation;
using CacheRepository.Implementation.Base;
using CacheRepository.SQLite.Data;
using Newtonsoft.Json;

namespace CacheRepository.SQLite
{
    // ReSharper disable once InconsistentNaming
    public class SQLiteCacheRepository : AsyncCacheRepositoryBase
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        private readonly Func<CacheContext> _contextFactory;

        public SQLiteCacheRepository()
            : this(() => new CacheContext())
        {
        }

        public SQLiteCacheRepository(Func<CacheContext> contextFactory)
            : base(DefaultCacheSettings.Instance)
        {
            _contextFactory = contextFactory;
        }

        public override async Task RemoveAsync(string key, CancellationToken cancelToken)
        {
            try
            {
                await _semaphore.WaitAsync(cancelToken).ConfigureAwait(false);

                using (var context = _contextFactory())
                {
                    var keyParam = new SQLiteParameter("@key", key);

                    await context.Database
                        .ExecuteSqlCommandAsync("DELETE FROM CacheEntry WHERE Key = @key", cancelToken, keyParam)
                        .ConfigureAwait(false);
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public override async Task ClearAllAsync(CancellationToken cancelToken)
        {
            try
            {
                await _semaphore.WaitAsync(cancelToken).ConfigureAwait(false);
                
                using (var context = _contextFactory())
                    await context.Database
                        .ExecuteSqlCommandAsync("DELETE FROM CacheEntry", cancelToken)
                        .ConfigureAwait(false);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        protected override async Task<Tuple<bool, T>> TryGetAsync<T>(string key, CancellationToken cancelToken)
        {
            CacheEntry entry;

            try
            {
                await _semaphore.WaitAsync(cancelToken).ConfigureAwait(false);

                using (var context = _contextFactory())
                    entry = await context.CacheEntries
                        .FirstOrDefaultAsync(c => c.Key == key, cancelToken)
                        .ConfigureAwait(false);
            }
            finally
            {
                _semaphore.Release();
            }

            if (entry == null)
                return Tuple.Create(false, default(T));

            if (entry.ExpireUtc.HasValue && entry.ExpireUtc.Value < DateTime.UtcNow)
            {
                await RemoveAsync(key, cancelToken).ConfigureAwait(false);
                return Tuple.Create(false, default(T));
            }
            
            var result = JsonConvert.DeserializeObject<T>(entry.Value);
            var slideSpan = entry.GetSlideSpan();

            if (slideSpan.HasValue)
                await SetAsync(key, result, slideSpan.Value, cancelToken).ConfigureAwait(false);

            return Tuple.Create(true, result);
        }

        protected override async Task SetAsync<T>(string key, T value, DateTime? expiration, TimeSpan? sliding, CancellationToken cancelToken)
        {
            var json = JsonConvert.SerializeObject(value);

            try
            {
                await _semaphore.WaitAsync(cancelToken).ConfigureAwait(false);
                
                using (var context = _contextFactory())
                {
                    var cacheEntry = await context.CacheEntries
                        .FirstOrDefaultAsync(c => c.Key == key, cancelToken)
                        .ConfigureAwait(false);

                    if (cacheEntry == null)
                    {
                        cacheEntry = new CacheEntry { Key = key };
                        context.CacheEntries.Add(cacheEntry);
                    }

                    if (sliding.HasValue)
                        expiration = DateTime.UtcNow.Add(sliding.Value);

                    cacheEntry.Value = json;
                    cacheEntry.ExpireUtc = expiration;
                    cacheEntry.SetSlideSpan(sliding);

                    await context
                        .SaveChangesAsync(cancelToken)
                        .ConfigureAwait(false);
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using CacheRepository.Configuration;

namespace CacheRepository.Implementation.Base
{
#pragma warning disable 612
    public abstract class AsyncCacheRepositoryBase : IAsyncCacheRepository, ICacheRepository
#pragma warning restore 612
    {
        private readonly ConcurrentDictionary<int, SemaphoreSlim> _semaphoreMap = new ConcurrentDictionary<int, SemaphoreSlim>();

        private readonly ICacheSettings _cacheSettings;

        protected AsyncCacheRepositoryBase(ICacheSettings cacheSettings)
        {
            _cacheSettings = cacheSettings;
        }

        public async Task<T> GetAsync<T>(string key, CancellationToken cancelToken)
        {
            var result = await TryGetAsync<T>(key, cancelToken).ConfigureAwait(false);
            return result.Item2;
        }

        public Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> loader, CancellationToken cancelToken)
        {
            return GetOrSetAsync(key, loader, null, null, cancelToken);
        }

        public Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> loader, DateTime expiration, CancellationToken cancelToken)
        {
            return GetOrSetAsync(key, loader, v => expiration, null, cancelToken);
        }

        public Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> loader, Func<T, DateTime> expiration, CancellationToken cancelToken)
        {
            return GetOrSetAsync(key, loader, expiration, null, cancelToken);
        }

        public Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> loader, TimeSpan sliding, CancellationToken cancelToken)
        {
            return GetOrSetAsync(key, loader, null, v => sliding, cancelToken);
        }

        public Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> loader, Func<T, TimeSpan> sliding, CancellationToken cancelToken)
        {
            return GetOrSetAsync(key, loader, null, sliding, cancelToken);
        }

        public Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> loader, CacheExpiration expiration, CancellationToken cancelToken)
        {
            return GetOrSetAsync(key, loader, v =>
            {
                var min = _cacheSettings.GetMinutes(expiration);
                return DateTime.UtcNow.AddMinutes(min);
            }, null, cancelToken);
        }

        public Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> loader, Func<T, CacheExpiration> expiration, CancellationToken cancelToken)
        {
            return GetOrSetAsync(key, loader, v =>
            {
                var enumValue = expiration(v);
                var min = _cacheSettings.GetMinutes(enumValue);
                return DateTime.UtcNow.AddMinutes(min);
            }, null, cancelToken);
        }

        public Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> loader, CacheSliding sliding, CancellationToken cancelToken)
        {
            return GetOrSetAsync(key, loader, null, v =>
            {
                var min = _cacheSettings.GetMinutes(sliding);
                return TimeSpan.FromMinutes(min);
            }, cancelToken);
        }

        public Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> loader, Func<T, CacheSliding> sliding, CancellationToken cancelToken)
        {
            return GetOrSetAsync(key, loader, null, v =>
            {
                var enumValue = sliding(v);
                var min = _cacheSettings.GetMinutes(enumValue);
                return TimeSpan.FromMinutes(min);
            }, cancelToken);
        }

        public Task SetAsync<T>(string key, T value, CancellationToken cancelToken)
        {
            return SetAsync(key, value, null, null, cancelToken);
        }

        public Task SetAsync<T>(string key, T value, DateTime expiration, CancellationToken cancelToken)
        {
            return SetAsync(key, value, expiration, null, cancelToken);
        }

        public Task SetAsync<T>(string key, T value, TimeSpan sliding, CancellationToken cancelToken)
        {
            return SetAsync(key, value, null, sliding, cancelToken);
        }

        public Task SetAsync<T>(string key, T value, CacheExpiration expiration, CancellationToken cancelToken)
        {
            var min = _cacheSettings.GetMinutes(expiration);
            var dateTime = DateTime.UtcNow.AddMinutes(min);
            return SetAsync(key, value, dateTime, null, cancelToken);
        }

        public Task SetAsync<T>(string key, T value, CacheSliding sliding, CancellationToken cancelToken)
        {
            var min = _cacheSettings.GetMinutes(sliding);
            var timeSpan = TimeSpan.FromMinutes(min);
            return SetAsync(key, value, null, timeSpan, cancelToken);
        }

        public abstract Task RemoveAsync(string key, CancellationToken cancelToken);

        public abstract Task ClearAllAsync(CancellationToken cancelToken);

        protected abstract Task SetAsync<T>(string key, T value, DateTime? expiration, TimeSpan? sliding, CancellationToken cancelToken);
        
        protected abstract Task<Tuple<bool, T>> TryGetAsync<T>(string key, CancellationToken cancelToken);

        private async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> loader, Func<T, DateTime> expirationFunc, Func<T, TimeSpan> slidingFunc, CancellationToken cancelToken)
        {
            // Get It
            var result = await TryGetAsync<T>(key, cancelToken).ConfigureAwait(false);

            // Got It or No Loader
            if (result.Item1 || loader == null)
                return result.Item2;

            // We do not want to create an infinite number of SemaphoreSlim objects, so instead
            // we has the key and take a mod to limit the max number of semaphores. This will 
            // obviously result in key colision, however it is extremely unlikely that will
            // happen during simultaneous get calls for multiple distinct keys.
            var keyHashMod = key.GetHashCode()%100;

            var semaphore = _semaphoreMap.GetOrAdd(keyHashMod, k => new SemaphoreSlim(1, 1));

            try
            {
                await semaphore.WaitAsync(cancelToken).ConfigureAwait(false);

                // Get It (Again)
                result = await TryGetAsync<T>(key, cancelToken).ConfigureAwait(false);

                if (result.Item1)
                    return result.Item2;

                // Load It (For Real)
                var value = await loader().ConfigureAwait(false);

                // This warning is disabled because a primitive type (int, bool, etc) will never 
                // be null, but that is okay because we want to cache those types of values.
                // ReSharper disable once CompareNonConstrainedGenericWithNull
                if (value != null)
                {
                    var expiration = expirationFunc == null
                        ? (DateTime?) null
                        : expirationFunc(value);

                    var sliding = slidingFunc == null
                        ? (TimeSpan?) null
                        : slidingFunc(value);

                    // Set It
                    await SetAsync(key, value, expiration, sliding, cancelToken).ConfigureAwait(false);
                }

                return value;
            }
            finally
            {
                semaphore.Release();
            }
        }

#pragma warning disable 612

        T ICacheRepository.Get<T>(string key)
        {
            var task = GetAsync<T>(key, CancellationToken.None);
            return task.Result;
        }

        T ICacheRepository.GetOrSet<T>(string key, Func<T> loader)
        {
            var task = GetOrSetAsync(key, () => Task.FromResult(loader()), CancellationToken.None);
            return task.Result;
        }

        T ICacheRepository.GetOrSet<T>(string key, Func<T> loader, DateTime expiration)
        {
            var task = GetOrSetAsync(key, () => Task.FromResult(loader()), expiration, CancellationToken.None);
            return task.Result;
        }

        T ICacheRepository.GetOrSet<T>(string key, Func<T> loader, Func<T, DateTime> expiration)
        {
            var task = GetOrSetAsync(key, () => Task.FromResult(loader()), expiration, CancellationToken.None);
            return task.Result;
        }

        T ICacheRepository.GetOrSet<T>(string key, Func<T> loader, TimeSpan sliding)
        {
            var task = GetOrSetAsync(key, () => Task.FromResult(loader()), sliding, CancellationToken.None);
            return task.Result;
        }

        T ICacheRepository.GetOrSet<T>(string key, Func<T> loader, Func<T, TimeSpan> sliding)
        {
            var task = GetOrSetAsync(key, () => Task.FromResult(loader()), sliding, CancellationToken.None);
            return task.Result;
        }

        T ICacheRepository.GetOrSet<T>(string key, Func<T> loader, CacheExpiration expiration)
        {
            var task = GetOrSetAsync(key, () => Task.FromResult(loader()), expiration, CancellationToken.None);
            return task.Result;
        }

        T ICacheRepository.GetOrSet<T>(string key, Func<T> loader, Func<T, CacheExpiration> expiration)
        {
            var task = GetOrSetAsync(key, () => Task.FromResult(loader()), expiration, CancellationToken.None);
            return task.Result;
        }

        T ICacheRepository.GetOrSet<T>(string key, Func<T> loader, CacheSliding sliding)
        {
            var task = GetOrSetAsync(key, () => Task.FromResult(loader()), sliding, CancellationToken.None);
            return task.Result;
        }

        T ICacheRepository.GetOrSet<T>(string key, Func<T> loader, Func<T, CacheSliding> sliding)
        {
            var task = GetOrSetAsync(key, () => Task.FromResult(loader()), sliding, CancellationToken.None);
            return task.Result;
        }

        void ICacheRepository.Set<T>(string key, T value)
        {
            var task = SetAsync(key, value, CancellationToken.None);
            task.Wait();
        }

        void ICacheRepository.Set<T>(string key, T value, DateTime expiration)
        {
            var task = SetAsync(key, value, expiration, CancellationToken.None);
            task.Wait();
        }

        void ICacheRepository.Set<T>(string key, T value, TimeSpan sliding)
        {
            var task = SetAsync(key, value, sliding, CancellationToken.None);
            task.Wait();
        }

        void ICacheRepository.Set<T>(string key, T value, CacheExpiration expiration)
        {
            var task = SetAsync(key, value, expiration, CancellationToken.None);
            task.Wait();
        }

        void ICacheRepository.Set<T>(string key, T value, CacheSliding sliding)
        {
            var task = SetAsync(key, value, sliding, CancellationToken.None);
            task.Wait();
        }

        void ICacheRepository.Remove(string key)
        {
            var task = RemoveAsync(key, CancellationToken.None);
            task.Wait();
        }

        void ICacheRepository.ClearAll()
        {
            var task = ClearAllAsync(CancellationToken.None);
            task.Wait();
        }

#pragma warning restore 612
    }
}
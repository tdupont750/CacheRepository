using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;

namespace CacheRepository.Implementation
{
    public abstract class CacheRepositoryBase : ICacheRepository
    {
        #region Abstract

        public abstract object Get(string key);

        protected abstract void Set(string key, object value, DateTime? expiration, TimeSpan? sliding);

        public abstract void Remove(string key);

        public abstract void ClearAll();

        #endregion
        
        #region GetOrSet

        private T GetOrSet<T>(string key, Func<T> loader, Func<T, DateTime> expirationFunc, Func<T, TimeSpan> slidingFunc)
        {
            // Get It
            T value;
            var success = TryGet(key, out value);

            // Got It or No Loader
            if (loader == null || success)
                return value;

            // Load It
            return LockedInvoke(key, () =>
            {
                // Get It (Again)
                success = TryGet(key, out value);

                if (!success)
                {
                    // Load It (For Real)
                    value = loader();

                    // Set It
                    // This is disabled because a primitive type (int, bool, etc) will never be
                    // null, but that is okay because we want to cache those types of values.
                    // ReSharper disable CompareNonConstrainedGenericWithNull
                    if (value != null)
                    {
                        var expiration = expirationFunc == null
                            ? (DateTime?) null
                            : expirationFunc(value);

                        var sliding = slidingFunc == null
                            ? (TimeSpan?) null
                            : slidingFunc(value);

                        Set(key, value, expiration, sliding);
                    }
                }

                return value;
            });
        }

        public T GetOrSet<T>(string key, Func<T> loader)
        {
            return GetOrSet(key, loader, null, null);
        }

        public T GetOrSet<T>(string key, Func<T> loader, DateTime expiration)
        {
            return GetOrSet(key, loader, v => expiration, null);
        }

        public T GetOrSet<T>(string key, Func<T> loader, Func<T, DateTime> expiration)
        {
            return GetOrSet(key, loader, expiration, null);
        }

        public T GetOrSet<T>(string key, Func<T> loader, TimeSpan sliding)
        {
            return GetOrSet(key, loader, null, v => sliding);
        }

        public T GetOrSet<T>(string key, Func<T> loader, Func<T, TimeSpan> sliding)
        {
            return GetOrSet(key, loader, null, sliding);
        }

        public T GetOrSet<T>(string key, Func<T> loader, CacheExpiration expiration)
        {
            return GetOrSet(key, loader, v => GetExpirationDateTime(expiration));
        }

        public T GetOrSet<T>(string key, Func<T> loader, Func<T, CacheExpiration> expiration)
        {
            return GetOrSet(key, loader, v => GetExpirationDateTime(expiration(v)));
        }

        public T GetOrSet<T>(string key, Func<T> loader, CacheSliding sliding)
        {
            return GetOrSet(key, loader, v => GetSlidingTimeSpan(sliding));
        }

        public T GetOrSet<T>(string key, Func<T> loader, Func<T, CacheSliding> sliding)
        {
            return GetOrSet(key, loader, v => GetSlidingTimeSpan(sliding(v)));
        }

        #endregion

        #region Set

        public void Set<T>(string key, T value)
        {
            Set(key, value, null, null);
        }

        public void Set<T>(string key, T value, DateTime expiration)
        {
            Set(key, value, expiration, null);
        }

        public void Set<T>(string key, T value, TimeSpan sliding)
        {
            Set(key, value, null, sliding);
        }

        public void Set<T>(string key, T value, CacheExpiration expiration)
        {
            var dt = GetExpirationDateTime(expiration);
            Set(key, value, dt);
        }

        public void Set<T>(string key, T value, CacheSliding sliding)
        {
            var ts = GetSlidingTimeSpan(sliding);
            Set(key, value, ts);
        }

        #endregion

        #region CacheExpiration

        private static readonly IDictionary<CacheExpiration, double> CacheExpirationMap = new ConcurrentDictionary<CacheExpiration, double>();

        private DateTime GetExpirationDateTime(CacheExpiration expiration)
        {
            var seconds = GetExpirationSeconds(expiration);
            return DateTime.Now.AddSeconds(seconds);
        }

        private double GetExpirationSeconds(CacheExpiration expiration)
        {
            if (CacheExpirationMap.ContainsKey(expiration))
                return CacheExpirationMap[expiration];

            var key = "CacheExpiration." + expiration;
            var settingValue = GetConfigurationValue(key);

            double resultValue;
            if (String.IsNullOrWhiteSpace(settingValue) || !Double.TryParse(settingValue, out resultValue))
                resultValue = (double)expiration;

            CacheExpirationMap[expiration] = resultValue;
            return resultValue;
        }

        #endregion

        #region CacheSliding

        private static readonly IDictionary<CacheSliding, double> CacheSlidingMap = new ConcurrentDictionary<CacheSliding, double>();

        private TimeSpan GetSlidingTimeSpan(CacheSliding expiration)
        {
            var seconds = GetSlidingSeconds(expiration);
            return TimeSpan.FromSeconds(seconds);
        }

        private double GetSlidingSeconds(CacheSliding sliding)
        {
            if (CacheSlidingMap.ContainsKey(sliding))
                return CacheSlidingMap[sliding];

            var key = "CacheSliding." + sliding;
            var settingValue = GetConfigurationValue(key);

            double resultValue;
            if (String.IsNullOrWhiteSpace(settingValue) || !Double.TryParse(settingValue, out resultValue))
                resultValue = (double)sliding;

            CacheSlidingMap[sliding] = resultValue;
            return resultValue;
        }

        #endregion

        #region Misc

        private readonly ConcurrentDictionary<string, object> _lockMap = new ConcurrentDictionary<string, object>();

        public T Get<T>(string key)
        {
            T value;
            TryGet(key, out value);
            return value;
        }
        
        private T LockedInvoke<T>(string key, Func<T> func)
        {
            var obj = _lockMap.GetOrAdd(key, new object());

            try
            {
                lock (obj)
                    return func();
            }
            finally
            {
                _lockMap.TryRemove(key, out obj);
            }
        }

        private bool TryGet<T>(string key, out T output)
        {
            var value = Get(key);
            if (value == null)
            {
                output = default(T);
                return false;
            }

            try
            {
                output = (T)value;
                return true;
            }
            catch (InvalidCastException)
            {
                output = default(T);
                return false;
            }
        }

        protected virtual string GetConfigurationValue(string key)
        {
            return ConfigurationManager.AppSettings.Get(key);
        }

        #endregion
    }
}
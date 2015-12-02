using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Linq;

namespace CacheRepository.Configuration.Implementation
{
    public class ConfigurationCacheSettings : ICacheSettings
    {
        public static readonly ConfigurationCacheSettings Instance = new ConfigurationCacheSettings();

        private ConfigurationCacheSettings()
        {
        }

        private readonly ConcurrentDictionary<CacheExpiration, int> _expirationMap = new ConcurrentDictionary<CacheExpiration, int>();

        private readonly ConcurrentDictionary<CacheSliding, int> _slidingMap = new ConcurrentDictionary<CacheSliding, int>();

        public int GetMinutes(CacheExpiration expiration)
        {
            return _expirationMap.GetOrAdd(expiration, e => GetMinutes(e, (int)e));
        }

        public int GetMinutes(CacheSliding sliding)
        {
            return _slidingMap.GetOrAdd(sliding, s => GetMinutes(s, (int) s));
        }

        private static int GetMinutes<T>(T enumValue, int defaultValue)
        {
            var type = typeof (T);
            var name = Enum.GetName(type, enumValue);
            var key = string.Format("{0}.{1}", type.Name, name);

            if (!ConfigurationManager.AppSettings.AllKeys.Contains(key)) 
                return defaultValue;
            
            int i;
            return int.TryParse(ConfigurationManager.AppSettings[key], out i)
                ? i 
                : defaultValue;
        }
    }
}
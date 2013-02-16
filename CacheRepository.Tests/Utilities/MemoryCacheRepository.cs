using System;
using System.Collections.Concurrent;
using CacheRepository.Implementation;

namespace CacheRepository.Tests.Utilities
{
    public class MemoryCacheRepository : CacheRepositoryBase
    {
        private readonly ConcurrentDictionary<string, object> _map = new ConcurrentDictionary<string, object>();

        public ConcurrentDictionary<string, object> Map
        {
            get { return _map; }
        }

        public DateTime? LastExpiration { get; private set; }

        public TimeSpan? LastSliding { get; private set; }

        public override object Get(string key)
        {
            object value;
            return _map.TryGetValue(key, out value)
                       ? value
                       : null;
        }

        protected override void Set(string key, object value, DateTime? expiration, TimeSpan? sliding)
        {
            LastExpiration = expiration;
            LastSliding = sliding;

            _map[key] = value;
        }

        public override void Remove(string key)
        {
            object value;
            _map.TryRemove(key, out value);
        }

        public override void ClearAll()
        {
            _map.Clear();
        }

        protected override string GetConfigurationValue(string key)
        {
            switch (key)
            {
                case "CacheExpiration.VeryLong":
                    return "3601";

                case "CacheSliding.VeryShort":
                    return "9";
                    
                default:
                    return String.Empty;
            }
        }
    }
}
using System;

namespace CacheRepository.Implementation
{
    public class EmptyCacheRepository : CacheRepositoryBase
    {
        public override object Get(string key)
        {
            return null;
        }

        protected override void Set(string key, object value, DateTime? expiration, TimeSpan? sliding)
        {
        }

        public override void Remove(string key)
        {
        }

        public override void ClearAll()
        {
        }
    }
}
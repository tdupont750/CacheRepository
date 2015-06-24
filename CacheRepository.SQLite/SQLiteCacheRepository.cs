using System;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using CacheRepository.Implementation;
using CacheRepository.SQLite.Data;
using Newtonsoft.Json;

namespace CacheRepository.SQLite
{
    // ReSharper disable once InconsistentNaming
    public class SQLiteCacheRepository : CacheRepositoryBase
    {
        private readonly object _contextLock = new object();

        private readonly Func<CacheContext> _contextFactory;

        public SQLiteCacheRepository()
            : this (() => new CacheContext())
        {
        }

        public SQLiteCacheRepository(Func<CacheContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public override object Get(string key)
        {
            CacheEntry entry;

            lock(_contextLock)
            using (var context = _contextFactory())
                entry = context.CacheEntries.FirstOrDefault(c => c.Key == key);

            if (entry == null)
                return null;

            var type = Type.GetType(entry.Type);
            if (type == null)
                throw new TypeAccessException("Type not found: " + entry.Type);

            return JsonConvert.DeserializeObject(entry.Value, type);
        }

        protected override void Set(string key, object value, DateTime? expiration, TimeSpan? sliding)
        {
            // TODO Support this.
            if (expiration.HasValue)
                throw new NotSupportedException("Expiration Not Supported");

            // TODO Support this.
            if (sliding.HasValue)
                throw new NotSupportedException("Sliding Not Supported");

            var json = JsonConvert.SerializeObject(value);
            var type = value.GetType().AssemblyQualifiedName;

            lock(_contextLock)
            using (var context = _contextFactory())
            {
                context.CacheEntries.Add(new CacheEntry
                {
                    Key = key,
                    Value = json,
                    Type = type,
                    CreatedUtc = DateTime.UtcNow,
                    ModifiedUtc = DateTime.UtcNow,
                });

                context.SaveChanges();
            }
        }

        public override void Remove(string key)
        {
            lock (_contextLock)
            using (var context = _contextFactory())
            {
                var keyParam = new SQLiteParameter("@key", key);
                context.Database.ExecuteSqlCommand("DELETE FROM CacheEntry WHERE Key = @key", keyParam);
            }
        }

        public override void ClearAll()
        {
            lock (_contextLock)
            using (var context = _contextFactory())
                context.Database.ExecuteSqlCommand("DELETE FROM CacheEntry");
        }
    }
}

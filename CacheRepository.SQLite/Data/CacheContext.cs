using System.Data.Entity;

namespace CacheRepository.SQLite.Data
{
    public class CacheContext : DbContext
    {
        public CacheContext()
            : base("CacheContext")
        {
        }

        public CacheContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        public DbSet<CacheEntry> CacheEntries { get; set; }
    }
}

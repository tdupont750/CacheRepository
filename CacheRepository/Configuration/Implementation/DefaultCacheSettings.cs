namespace CacheRepository.Configuration.Implementation
{
    public class DefaultCacheSettings : ICacheSettings
    {
        public static readonly DefaultCacheSettings Instance = new DefaultCacheSettings();
        
        public int GetMinutes(CacheExpiration expiration)
        {
            return (int)expiration;
        }

        public int GetMinutes(CacheSliding sliding)
        {
            return (int)sliding;
        }
    }
}
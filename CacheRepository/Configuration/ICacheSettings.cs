namespace CacheRepository.Configuration
{
    public interface ICacheSettings
    {
        int GetMinutes(CacheExpiration expiration);

        int GetMinutes(CacheSliding sliding);
    }
}
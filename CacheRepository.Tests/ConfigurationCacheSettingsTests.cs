using CacheRepository.Configuration.Implementation;
using Xunit;

namespace CacheRepository.Tests
{
    public class ConfigurationCacheSettingsTests
    {
        [Theory]
        [InlineData(CacheExpiration.VeryShort, null)]
        [InlineData(CacheExpiration.Short, null)]
        [InlineData(CacheExpiration.Medium, null)]
        [InlineData(CacheExpiration.Long, null)]
        [InlineData(CacheExpiration.VeryLong, 9999)]
        public void Expiration(CacheExpiration expiration, int? expected)
        {
            var actual = ConfigurationCacheSettings.Instance.GetMinutes(expiration);
            Assert.Equal(expected ?? (int) expiration, actual);
        }

        [Theory]
        [InlineData(CacheSliding.VeryShort, 999)]
        [InlineData(CacheSliding.Short, null)]
        [InlineData(CacheSliding.Medium, null)]
        [InlineData(CacheSliding.Long, null)]
        [InlineData(CacheSliding.VeryLong, null)]
        public void Sliding(CacheSliding sliding, int? expected)
        {
            var actual = ConfigurationCacheSettings.Instance.GetMinutes(sliding);
            Assert.Equal(expected ?? (int)sliding, actual);
        }
    }
}
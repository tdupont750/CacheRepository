using System;
using CacheRepository.Tests.Utilities;
using Xunit;
using Xunit.Extensions;

namespace CacheRepository.Tests
{
    public class ExpirationTests : CacheRepositoryTestsBase
    {
        [Theory]
        [InlineData(CacheExpiration.VeryShort, null)]
        [InlineData(CacheExpiration.Short, null)]
        [InlineData(CacheExpiration.Medium, null)]
        [InlineData(CacheExpiration.Long, null)]
        [InlineData(CacheExpiration.VeryLong, 3601)]
        public void Expiration(CacheExpiration expiration, int? addSeconds)
        {
            CacheRepository.Set("Key", 1, expiration);
            var lastExpiration = DateTime.Now.AddSeconds(addSeconds ?? (int)expiration);
            Assert.Equal(lastExpiration.ToString(), MemoryCacheRepository.LastExpiration.ToString());
        }

        [Theory]
        [InlineData(CacheSliding.VeryShort, 9)]
        [InlineData(CacheSliding.Short, null)]
        [InlineData(CacheSliding.Medium, null)]
        [InlineData(CacheSliding.Long, null)]
        [InlineData(CacheSliding.VeryLong, null)]
        public void Sliding(CacheSliding sliding, int? addSeconds)
        {
            CacheRepository.Set("Key", 1, sliding);
            var lastSliding = TimeSpan.FromSeconds(addSeconds ?? (int)sliding);
            Assert.Equal(lastSliding.ToString(), MemoryCacheRepository.LastSliding.ToString());
        }
    }
}
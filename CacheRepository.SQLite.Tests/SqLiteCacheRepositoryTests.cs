using System;
using System.Linq;
using System.Threading.Tasks;
using CacheRepository.SQLite.Data;
using Xunit;

namespace CacheRepository.SQLite.Tests
{
// ReSharper disable once InconsistentNaming
    public class SQLiteCacheRepositoryTests : IDisposable
    {
        static SQLiteCacheRepositoryTests()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Environment.CurrentDirectory);
        }

        private readonly Func<CacheContext> _contextFactory;

        private readonly IAsyncCacheRepository _cache;

        public SQLiteCacheRepositoryTests()
        {
            _contextFactory = () => new CacheContext();
            _cache = new SQLiteCacheRepository(_contextFactory);

            _cache.ClearAllAsync().Wait();
        }

        public void Dispose()
        {
            _cache.ClearAllAsync().Wait();
        }

        [Fact]
        public async Task GuidTest()
        {
            const string key = "Guid1";
            var setValue = Guid.NewGuid();

            var getValue1 = await _cache.GetOrSetAsync(key, () => Task.FromResult(setValue));
            Assert.Equal(setValue, getValue1);
            AssertCount(1);

            var getValue2 = await _cache.GetOrSetAsync(key, () => Task.FromResult(setValue));
            Assert.Equal(setValue, getValue2);
            AssertCount(1);
        }

        [Fact]
        public async Task ObjectTest()
        {
            const string key = "Object";
            var setValue = new Sample { Age = 42, Name = key };

            var getValue1 = await _cache.GetOrSetAsync(key, () => Task.FromResult(setValue));
            Assert.Equal(setValue.Age, getValue1.Age);
            Assert.Equal(setValue.Name, getValue1.Name);
            AssertCount(1);

            var getValue2 = await _cache.GetOrSetAsync(key, () => Task.FromResult(setValue));
            Assert.Equal(setValue.Age, getValue2.Age);
            Assert.Equal(setValue.Name, getValue2.Name);
            AssertCount(1);
        }

        [Fact]
        public async Task ExpirationTest()
        {
            const string key = "Expiration";
            var setValue = new Sample { Age = 50, Name = key };
            var timeSpan = TimeSpan.FromSeconds(1);

            await _cache.SetAsync(key, setValue, DateTime.Now.Add(timeSpan));

            var getValue1 = await _cache.GetAsync<Sample>(key);
            Assert.NotNull(getValue1);
            AssertCount(1);

            await Task.Delay(timeSpan);

            var getValue2 = await _cache.GetAsync<Sample>(key);
            Assert.Null(getValue2);
            AssertCount(0);
        }

        [Fact]
        public async Task SlidingTest()
        {
            const string key = "Sliding";
            var setValue = new Sample { Age = 50, Name = key };
            var fullSpan = TimeSpan.FromMilliseconds(1000);
            var halfSpan = TimeSpan.FromMilliseconds(500);

            await _cache.SetAsync(key, setValue, fullSpan);
            AssertCount(1);

            var getValue1 = await _cache.GetAsync<Sample>(key);
            Assert.NotNull(getValue1);
            Assert.Equal(setValue.Age, getValue1.Age);
            Assert.Equal(setValue.Name, getValue1.Name);
            AssertCount(1);

            await Task.Delay(halfSpan);

            var getValue2 = await _cache.GetAsync<Sample>(key);
            Assert.NotNull(getValue2);
            Assert.Equal(setValue.Age, getValue2.Age);
            Assert.Equal(setValue.Name, getValue2.Name);
            AssertCount(1);

            await Task.Delay(halfSpan);

            var getValue3 = await _cache.GetAsync<Sample>(key);
            Assert.NotNull(getValue3);
            Assert.Equal(setValue.Age, getValue3.Age);
            Assert.Equal(setValue.Name, getValue3.Name);
            AssertCount(1);

            await Task.Delay(fullSpan);

            var getValue4 = await _cache.GetAsync<Sample>(key);
            Assert.Null(getValue4);
            AssertCount(0);
        }

        [Fact]
        public async Task RemoveTest()
        {
            const string key = "Object";
            var setValue = new Sample { Age = 42, Name = key };

            await _cache.SetAsync(key, setValue);

            var getValue1 = await _cache.GetAsync<Sample>(key);
            Assert.NotNull(getValue1);
            Assert.Equal(setValue.Age, getValue1.Age);
            Assert.Equal(setValue.Name, getValue1.Name);
            AssertCount(1);

            await _cache.RemoveAsync(key);
            AssertCount(0);

            var getValue2 = await _cache.GetAsync<Sample>(key);
            Assert.Null(getValue2);
        }

        private void AssertCount(int expected)
        {
            int count;

            using (var context = _contextFactory())
                count = context.CacheEntries.Count();

            Assert.Equal(expected, count);
        }

        public class Sample
        {
            public int Age { get; set; }
            public string Name { get; set; }
        }
    }
}

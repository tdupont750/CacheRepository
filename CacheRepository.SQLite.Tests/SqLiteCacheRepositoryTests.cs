using System;
using System.IO;
using System.Linq;
using System.Reflection;
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

        private readonly ICacheRepository _cache;

        public SQLiteCacheRepositoryTests()
        {
            _contextFactory = () => new CacheContext();
            _cache = new SQLiteCacheRepository(_contextFactory);

            _cache.ClearAll();

        }

        public void Dispose()
        {
            _cache.ClearAll();
        }

        [Fact]
        public void GuidTest()
        {
            const string key = "Guid1";
            var setValue = Guid.NewGuid();

            var getValue1 = _cache.GetOrSet(key, () => setValue);
            Assert.Equal(setValue, getValue1);
            AssertCount(1);
            
            var getValue2 = _cache.GetOrSet(key, () => setValue);
            Assert.Equal(setValue, getValue2);
            AssertCount(1);
        }

        [Fact]
        public void ObjectTest()
        {
            const string key = "Object";
            var setValue = new Sample {Age = 42, Hello = "World"};

            var getValue1 = _cache.GetOrSet(key, () => setValue);
            Assert.Equal(setValue.Age, getValue1.Age);
            Assert.Equal(setValue.Hello, getValue1.Hello);
            AssertCount(1);

            var getValue2 = _cache.GetOrSet(key, () => setValue);
            Assert.Equal(setValue.Age, getValue2.Age);
            Assert.Equal(setValue.Hello, getValue2.Hello);
            AssertCount(1);
        }

        [Fact]
        public void RemoveTest()
        {
            const string key = "Object";
            var setValue = new Sample { Age = 42, Hello = "World" };

            var getValue1 = _cache.GetOrSet(key, () => setValue);
            Assert.Equal(setValue.Age, getValue1.Age);
            Assert.Equal(setValue.Hello, getValue1.Hello);
            AssertCount(1);

            _cache.Remove(key);
            AssertCount(0);
        }

        private void AssertCount(int expected)
        {
            using (var context = _contextFactory())
            {
                var count = context.CacheEntries.Count();
                Assert.Equal(expected, count);
            }
        }

        public class Sample
        {
            public int Age { get; set; }
            public string Hello { get; set; }
        }
    }
}

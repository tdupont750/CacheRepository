using System.Collections.Generic;
using System.Linq;
using LinqToSql.Futures.Tests.Data;
using Xunit;

namespace LinqToSql.Futures.Tests.Tests
{
    public class NonDataQueryTests
    {
        [Fact]
        public void NonDataQuery()
        {
            var list = new List<int> { 1, 2, 3 };
            var query = list.AsQueryable();
            var lazy = query.ToFuture();

            Assert.NotNull(lazy);
            Assert.False(lazy.IsValueCreated);

            var value = lazy.Value;

            Assert.NotNull(value);
            Assert.True(lazy.IsValueCreated);
            Assert.Equal(3, value.Count);
        }

        [Fact]
        public void NonDataQueryWithDataContext()
        {
            var list = new List<int> { 1, 2, 3 };
            var query = list.AsQueryable();

            using (var dataContext = new FutureSimpleDataContext())
            {
                var lazy = query.ToFuture(dataContext);

                Assert.NotNull(lazy);
                Assert.False(lazy.IsValueCreated);
                Assert.Equal(0, dataContext.FutureCollection.Count);

                var value = lazy.Value;

                Assert.NotNull(value);
                Assert.True(lazy.IsValueCreated);
                Assert.Equal(3, value.Count);
                Assert.Equal(0, dataContext.FutureCollection.Count);
            }
        }

        [Fact]
        public void NonDataQueryWithCollection()
        {
            var list = new List<int> { 1, 2, 3 };
            var query = list.AsQueryable();

            using (var dataContext = new SimpleDataContext())
            using (var futureCollection = dataContext.CreateFutureCollection())
            {
                var lazy = query.ToFuture(futureCollection);

                Assert.NotNull(lazy);
                Assert.False(lazy.IsValueCreated);
                Assert.Equal(0, futureCollection.Count);

                var value = lazy.Value;

                Assert.NotNull(value);
                Assert.True(lazy.IsValueCreated);
                Assert.Equal(3, value.Count);
                Assert.Equal(0, futureCollection.Count);
            }
        }
    }
}

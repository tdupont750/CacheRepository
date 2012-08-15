using System;
using System.Linq;
using LinqToSql.Futures.Tests.Data;
using Xunit;

namespace LinqToSql.Futures.Tests.Tests
{
    public class SimpleTests
    {
        [Fact]
        public void ToFuture()
        {
            using (var dataContext = new SimpleDataContext())
            {
                Assert.Throws<InvalidCastException>(() =>
                {
                    dataContext.Persons
                        .Where(a => a.FirstName == "Tom" || a.FirstName == "Cat")
                        .ToFuture();
                });
            }
        }
        
        [Fact]
        public void ToFutureWithCollection()
        {
            using (var dataContext = new SimpleDataContext())
            using (var futureCollection = dataContext.CreateFutureCollection())
            {
                var people = dataContext.Persons
                    .Where(a => a.FirstName == "Tom" || a.FirstName == "Cat")
                    .ToFuture(futureCollection);

                Assert.NotNull(people);
                Assert.Equal(1, futureCollection.Count);

                var pets = dataContext.Pets
                    .Where(a => a.Name == "Taboo")
                    .ToFuture(futureCollection);

                Assert.NotNull(pets);
                Assert.Equal(2, futureCollection.Count);

                var peopleValues = people.Value;

                Assert.NotNull(peopleValues);
                Assert.True(people.IsValueCreated);
                Assert.False(pets.IsValueCreated);
                Assert.Equal(2, peopleValues.Count);
                Assert.Equal(0, futureCollection.Count);
            }
        }
    }
}
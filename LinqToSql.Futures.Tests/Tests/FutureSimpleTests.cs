using System.Linq;
using LinqToSql.Futures.Tests.Data;
using Xunit;

namespace LinqToSql.Futures.Tests.Tests
{
    public class FutureSimpleTests
    {
        [Fact]
        public void ToFuture()
        {
            using (var dataContext = new FutureSimpleDataContext())
            {
                var people = dataContext.FuturePersons
                    .Where(a => a.FirstName == "Tom" || a.FirstName == "Cat")
                    .ToFuture();

                Assert.NotNull(people);
                Assert.False(people.IsValueCreated);
                Assert.Equal(1, dataContext.FutureCollection.Count);

                var pets = dataContext.FuturePets
                    .Where(a => a.Name == "Taboo")
                    .ToFuture();

                Assert.NotNull(pets);
                Assert.False(pets.IsValueCreated);
                Assert.Equal(2, dataContext.FutureCollection.Count);

                var peopleValues = people.Value;

                Assert.NotNull(peopleValues);
                Assert.True(people.IsValueCreated);
                Assert.False(pets.IsValueCreated);
                Assert.Equal(2, peopleValues.Count);
                Assert.Equal(0, dataContext.FutureCollection.Count);
            }
        }

        [Fact]
        public void ToFutureWithDataContext()
        {
            using (var dataContext = new FutureSimpleDataContext())
            {
                var people = dataContext.FuturePersons
                    .Where(a => a.FirstName == "Tom" || a.FirstName == "Cat")
                    .ToFuture(dataContext);

                Assert.NotNull(people);
                Assert.False(people.IsValueCreated);
                Assert.Equal(1, dataContext.FutureCollection.Count);

                var pets = dataContext.FuturePets
                    .Where(a => a.Name == "Taboo")
                    .ToFuture(dataContext);

                Assert.NotNull(pets);
                Assert.False(pets.IsValueCreated);
                Assert.Equal(2, dataContext.FutureCollection.Count);

                var peopleValues = people.Value;

                Assert.NotNull(peopleValues);
                Assert.True(people.IsValueCreated);
                Assert.False(pets.IsValueCreated);
                Assert.Equal(2, peopleValues.Count);
                Assert.Equal(0, dataContext.FutureCollection.Count);
            }
        }

        [Fact]
        public void ToFutureWithCollection()
        {
            using (var dataContext = new FutureSimpleDataContext())
            using (var futureCollection = dataContext.CreateFutureCollection())
            {
                var people = dataContext.FuturePersons
                    .Where(a => a.FirstName == "Tom" || a.FirstName == "Cat")
                    .ToFuture(futureCollection);

                Assert.NotNull(people);
                Assert.False(people.IsValueCreated);
                Assert.Equal(0, dataContext.FutureCollection.Count);
                Assert.Equal(1, futureCollection.Count);

                var pets = dataContext.FuturePets
                    .Where(a => a.Name == "Taboo")
                    .ToFuture(futureCollection);

                Assert.NotNull(pets);
                Assert.False(pets.IsValueCreated);
                Assert.Equal(0, dataContext.FutureCollection.Count);
                Assert.Equal(2, futureCollection.Count);

                var peopleValues = people.Value;

                Assert.NotNull(peopleValues);
                Assert.True(people.IsValueCreated);
                Assert.False(pets.IsValueCreated);
                Assert.Equal(2, peopleValues.Count);
                Assert.Equal(0, dataContext.FutureCollection.Count);
                Assert.Equal(0, futureCollection.Count);
            }
        }
    }
}

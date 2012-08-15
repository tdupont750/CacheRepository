using System.Linq;
using LinqToSql.Futures.Tests.Data;
using Xunit;

namespace LinqToSql.Futures.Tests.Tests
{
    public class FutureCollectionTests
    {
        [Fact]
        public void Execute()
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

                dataContext.FutureCollection.Execute();

                Assert.False(people.IsValueCreated);
                Assert.False(pets.IsValueCreated);
                Assert.Equal(0, dataContext.FutureCollection.Count);
            }
        }

        [Fact]
        public void IsolationLevel()
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

                dataContext.FutureCollection.Execute(System.Data.IsolationLevel.ReadUncommitted);

                Assert.False(people.IsValueCreated);
                Assert.False(pets.IsValueCreated);
                Assert.Equal(0, dataContext.FutureCollection.Count);
            }
        }

        [Fact]
        public void AddQuery()
        {
            using (var dataContext = new FutureSimpleDataContext())
            {
                var peopleQuery = dataContext.FuturePersons.Where(a => a.FirstName == "Tom" || a.FirstName == "Cat");
                var peopleFuture = dataContext.FutureCollection.Add(peopleQuery);
                var people = peopleFuture.ToLazy(dataContext.FutureCollection);

                Assert.NotNull(people);
                Assert.False(peopleFuture.IsValueLoaded);
                Assert.False(people.IsValueCreated);

                Assert.Equal(1, dataContext.FutureCollection.Count);

                var petsQuery = dataContext.FuturePets.Where(a => a.Name == "Taboo");
                var petsFuture = dataContext.FutureCollection.Add(petsQuery);
                var pets = petsFuture.ToLazy(dataContext.FutureCollection);

                Assert.NotNull(pets);
                Assert.False(petsFuture.IsValueLoaded);
                Assert.False(pets.IsValueCreated);

                Assert.Equal(2, dataContext.FutureCollection.Count);

                var petsValue = pets.Value;

                Assert.True(peopleFuture.IsValueLoaded);
                Assert.Equal(2, peopleFuture.Value.Count);
                Assert.False(people.IsValueCreated);

                Assert.True(petsFuture.IsValueLoaded);
                Assert.Equal(1, petsFuture.Value.Count);
                Assert.True(pets.IsValueCreated);
                Assert.Equal(1, petsValue.Count);

                Assert.Equal(0, dataContext.FutureCollection.Count);
            }
        }

        [Fact]
        public void AddQueryExecute()
        {
            using (var dataContext = new FutureSimpleDataContext())
            {
                var peopleQuery = dataContext.FuturePersons.Where(a => a.FirstName == "Tom" || a.FirstName == "Cat");
                var peopleFuture = dataContext.FutureCollection.Add(peopleQuery);
                var people = peopleFuture.ToLazy(dataContext.FutureCollection);

                Assert.NotNull(people);
                Assert.False(peopleFuture.IsValueLoaded);
                Assert.False(people.IsValueCreated);

                Assert.Equal(1, dataContext.FutureCollection.Count);

                var petsQuery = dataContext.FuturePets.Where(a => a.Name == "Taboo");
                var petsFuture = dataContext.FutureCollection.Add(petsQuery);
                var pets = petsFuture.ToLazy(dataContext.FutureCollection);

                Assert.NotNull(pets);
                Assert.False(petsFuture.IsValueLoaded);
                Assert.False(pets.IsValueCreated);

                Assert.Equal(2, dataContext.FutureCollection.Count);

                dataContext.FutureCollection.Execute();

                Assert.True(peopleFuture.IsValueLoaded);
                Assert.Equal(2, peopleFuture.Value.Count);
                Assert.False(people.IsValueCreated);

                Assert.True(petsFuture.IsValueLoaded);
                Assert.Equal(1, petsFuture.Value.Count);
                Assert.False(pets.IsValueCreated);

                Assert.Equal(0, dataContext.FutureCollection.Count);
            }
        }
    }
}

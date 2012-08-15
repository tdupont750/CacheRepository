using System.Collections.Generic;
using System.Linq;
using LinqToSql.Futures.Tests.Data;
using Xunit;

namespace LinqToSql.Futures.Tests.Tests
{
    public class MiscTests
    {
        [Fact]
        public void LotsOfParams()
        {
            using (var dataContext = new FutureSimpleDataContext())
            {
                var people = dataContext.FuturePersons
                    .Where(a => a.FirstName == "Tom" || a.FirstName == "Cat")
                    .ToFuture();

                Assert.NotNull(people);
                Assert.False(people.IsValueCreated);
                Assert.Equal(1, dataContext.FutureCollection.Count);

                var petIds = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

                var pets = dataContext.FuturePets
                    .Where(p => petIds.Contains(p.Id))
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
    }
}

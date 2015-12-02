using System.Threading.Tasks;
using CacheRepository.Tests.Utilities;
using Xunit;

namespace CacheRepository.Tests
{
    public class SetByTypeTests : CacheRepositoryTestsBase
    {
        [Fact]
        public async Task SameKeyDifferentType()
        {
            var setCat = new Utilities.One.Cat { Name = "Linq" };
            await CacheRepository.SetByTypeAsync(1, setCat);

            var setDog = new Utilities.One.Dog { Name = "Taboo" };
            await CacheRepository.SetByTypeAsync(1, setDog);

            var getCat = await CacheRepository.GetByTypeAsync<Utilities.One.Cat>(1);
            Assert.Equal(setCat.Name, getCat.Name);

            var getDog = await CacheRepository.GetByTypeAsync<Utilities.One.Dog>(1);
            Assert.Equal(setDog.Name, getDog.Name);

            Assert.Equal(2, MemoryCacheRepository.Map.Count);
        }

        [Fact]
        public async Task SameTypeDifferentNamespace()
        {
            var setCat1 = new Utilities.One.Cat { Name = "Linq" };
            await CacheRepository.SetByTypeAsync(1, setCat1);

            var setCat2 = new Utilities.Two.Cat { Name = "Sql" };
            await CacheRepository.SetByTypeAsync(1, setCat2);

            var getCat1 = await CacheRepository.GetByTypeAsync<Utilities.One.Cat>(1);
            Assert.Equal(setCat1.Name, getCat1.Name);

            var getCat2 = await CacheRepository.GetByTypeAsync<Utilities.Two.Cat>(1);
            Assert.Equal(setCat2.Name, getCat2.Name);

            Assert.Equal(2, MemoryCacheRepository.Map.Count);
        }

        [Fact]
        public async Task SameTypeDifferentKey()
        {
            var setCatA = new Utilities.One.Cat { Name = "Linq" };
            await CacheRepository.SetByTypeAsync(1, setCatA);

            var setCatB = new Utilities.One.Cat { Name = "Sql" };
            await CacheRepository.SetByTypeAsync(2, setCatB);

            var getCatA = await CacheRepository.GetByTypeAsync<Utilities.One.Cat>(1);
            Assert.Equal(setCatA.Name, getCatA.Name);

            var getCatB = await CacheRepository.GetByTypeAsync<Utilities.One.Cat>(2);
            Assert.Equal(setCatB.Name, getCatB.Name);

            Assert.Equal(2, MemoryCacheRepository.Map.Count);
        }

        [Fact]
        public async Task SameTypeSameKey()
        {
            var setCatA = new Utilities.One.Cat { Name = "Linq" };
            await CacheRepository.SetByTypeAsync(1, setCatA);

            var setCatB = new Utilities.One.Cat { Name = "Sql" };
            await CacheRepository.SetByTypeAsync(1, setCatB);

            var getCat = await CacheRepository.GetByTypeAsync<Utilities.One.Cat>(1);
            Assert.Equal(setCatB.Name, getCat.Name);

            Assert.Equal(1, MemoryCacheRepository.Map.Count);
        }

        [Fact]
        public async Task DerivedTypeSameKey()
        {
            var setDogA = new Utilities.One.Dog { Name = "Taboo" };
            await CacheRepository.SetByTypeAsync(1, setDogA);

            var setDogB = new Utilities.Two.Dog { Name = "Jay" };
            await CacheRepository.SetByTypeAsync(1, setDogB);

            var getDogA = await CacheRepository.GetByTypeAsync<Utilities.One.Dog>(1);
            Assert.Equal(setDogA.Name, getDogA.Name);

            var getDogB = await CacheRepository.GetByTypeAsync<Utilities.Two.Dog>(1);
            Assert.Equal(setDogB.Name, getDogB.Name);

            Assert.Equal(2, MemoryCacheRepository.Map.Count);
        }
    }
}

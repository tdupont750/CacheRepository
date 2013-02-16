using CacheRepository.Tests.Utilities;
using Xunit;

namespace CacheRepository.Tests
{
    public class SetByTypeTests : CacheRepositoryTestsBase
    {
        [Fact]
        public void SameKeyDifferentType()
        {
            var setCat = new Utilities.One.Cat { Name = "Linq" };
            CacheRepository.SetByType(1, setCat);

            var setDog = new Utilities.One.Dog { Name = "Taboo" };
            CacheRepository.SetByType(1, setDog);

            var getCat = CacheRepository.GetByType<Utilities.One.Cat>(1);
            Assert.Equal(setCat.Name, getCat.Name);

            var getDog = CacheRepository.GetByType<Utilities.One.Dog>(1);
            Assert.Equal(setDog.Name, getDog.Name);

            Assert.Equal(2, MemoryCacheRepository.Map.Count);
        }

        [Fact]
        public void SameTypeDifferentNamespace()
        {
            var setCat1 = new Utilities.One.Cat { Name = "Linq" };
            CacheRepository.SetByType(1, setCat1);

            var setCat2 = new Utilities.Two.Cat { Name = "Sql" };
            CacheRepository.SetByType(1, setCat2);

            var getCat1 = CacheRepository.GetByType<Utilities.One.Cat>(1);
            Assert.Equal(setCat1.Name, getCat1.Name);

            var getCat2 = CacheRepository.GetByType<Utilities.Two.Cat>(1);
            Assert.Equal(setCat2.Name, getCat2.Name);

            Assert.Equal(2, MemoryCacheRepository.Map.Count);
        }

        [Fact]
        public void SameTypeDifferentKey()
        {
            var setCatA = new Utilities.One.Cat { Name = "Linq" };
            CacheRepository.SetByType(1, setCatA);

            var setCatB = new Utilities.One.Cat { Name = "Sql" };
            CacheRepository.SetByType(2, setCatB);

            var getCatA = CacheRepository.GetByType<Utilities.One.Cat>(1);
            Assert.Equal(setCatA.Name, getCatA.Name);

            var getCatB = CacheRepository.GetByType<Utilities.One.Cat>(2);
            Assert.Equal(setCatB.Name, getCatB.Name);

            Assert.Equal(2, MemoryCacheRepository.Map.Count);
        }

        [Fact]
        public void SameTypeSameKey()
        {
            var setCatA = new Utilities.One.Cat { Name = "Linq" };
            CacheRepository.SetByType(1, setCatA);

            var setCatB = new Utilities.One.Cat { Name = "Sql" };
            CacheRepository.SetByType(1, setCatB);

            var getCat = CacheRepository.GetByType<Utilities.One.Cat>(1);
            Assert.Equal(setCatB.Name, getCat.Name);

            Assert.Equal(1, MemoryCacheRepository.Map.Count);
        }

        [Fact]
        public void DerivedTypeSameKey()
        {
            var setDogA = new Utilities.One.Dog { Name = "Taboo" };
            CacheRepository.SetByType(1, setDogA);

            var setDogB = new Utilities.Two.Dog { Name = "Jay" };
            CacheRepository.SetByType(1, setDogB);

            var getDogA = CacheRepository.GetByType<Utilities.One.Dog>(1);
            Assert.Equal(setDogA.Name, getDogA.Name);

            var getDogB = CacheRepository.GetByType<Utilities.Two.Dog>(1);
            Assert.Equal(setDogB.Name, getDogB.Name);

            Assert.Equal(2, MemoryCacheRepository.Map.Count);
        }
    }
}

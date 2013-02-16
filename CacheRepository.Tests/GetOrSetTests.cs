using System.Collections.Generic;
using System.Threading.Tasks;
using CacheRepository.Tests.Utilities;
using Xunit;

namespace CacheRepository.Tests
{
    public class GetOrSetTests : CacheRepositoryTestsBase
    {
        [Fact]
        public void ParallelForEach()
        {
            var numbers = new List<int>();
            for(var i=0; i<100; i++)
                numbers.Add(i);

            var activations = 0;
            var options = new ParallelOptions {MaxDegreeOfParallelism = 100};
            Parallel.ForEach(numbers, options, i => CacheRepository.GetOrSet("Key", () =>
            {
                activations++;
                return i;
            }));

            Assert.Equal(1, activations);
            Assert.Equal(1, MemoryCacheRepository.Map.Count);
        }
    }
}
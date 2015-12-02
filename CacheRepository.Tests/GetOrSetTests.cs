using System.Linq;
using System.Threading;
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
            var numbers = Enumerable.Range(0, 100).ToList();

            var activations = 0;
            var options = new ParallelOptions {MaxDegreeOfParallelism = 100};
            Parallel.ForEach(numbers, options, i => CacheRepository.GetOrSetAsync("Key", () =>
            {
                Interlocked.Increment(ref activations);
                return Task.FromResult(i);
            }).Wait());

            Assert.Equal(1, activations);
            Assert.Equal(1, MemoryCacheRepository.Map.Count);
        }
    }
}
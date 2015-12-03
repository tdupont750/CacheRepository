using System.Threading.Tasks;
using CacheRepository.Threading;
using Xunit;

namespace CacheRepository.Tests
{
    public class KeyedSemaphoreSlimTests
    {
        [Fact]
        public async Task WaitAsync()
        {
            using (var semaphore = new KeyedSemaphoreSlim())
            {
                // Use three keys to create three semaphores.
                var ta1 = semaphore.WaitAsync("A");
                var tb1 = semaphore.WaitAsync("B");
                var ta2 = semaphore.WaitAsync("A");
                var tb2 = semaphore.WaitAsync("B");
                var ta3 = semaphore.WaitAsync("A");
                var tc1 = semaphore.WaitAsync("C");

                // Assert that first entry for each key is complete.
                Assert.True(ta1.IsCompleted);
                Assert.True(tb1.IsCompleted);
                Assert.False(ta2.IsCompleted);
                Assert.False(tb2.IsCompleted);
                Assert.False(ta3.IsCompleted);
                Assert.True(tc1.IsCompleted);

                // Complete the first entry by disposing.
                var ra1 = await ta1;
                ra1.Dispose();
                var rb1 = await tb1;
                rb1.Dispose();
                var rc1 = await tc1;
                rc1.Dispose();
                await Task.Delay(20);

                // Show that the second entry is now complete.
                Assert.True(ta2.IsCompleted);
                Assert.True(tb2.IsCompleted);
                Assert.False(ta3.IsCompleted);

                // Complete the second entry by disposing.
                var ra2 = await ta2;
                ra2.Dispose();
                var rb2 = await tb2;
                rb2.Dispose();
                await Task.Delay(20);

                // Show that the third entry is now complete.
                Assert.True(ta3.IsCompleted);

                // Complete the third entry by disposing.
                var ra3 = await ta3;
                ra3.Dispose();
                await Task.Delay(20);

                // Assert that each key shares a unique semaphore instance.
                Assert.Same(ra1, ra2);
                Assert.Same(ra2, ra3);
                Assert.Same(rb1, rb2);
                Assert.NotSame(ra1, rb1);
                Assert.NotSame(ra1, rc1);

                // Get four new keys.
                var td1 = semaphore.WaitAsync("D");
                var te1 = semaphore.WaitAsync("E");
                var tf1 = semaphore.WaitAsync("F");
                var tg1 = semaphore.WaitAsync("G");

                // Assert that they are all complete.
                Assert.True(td1.IsCompleted);
                Assert.True(te1.IsCompleted);
                Assert.True(tf1.IsCompleted);
                Assert.True(tg1.IsCompleted);

                // Complete the first = entry for each.
                var rd1 = await td1;
                rd1.Dispose();
                var re1 = await te1;
                re1.Dispose();
                var rf1 = await tf1;
                rf1.Dispose();
                var rg1 = await tg1;
                rg1.Dispose();
                await Task.Delay(20);

                // Assert that the first three reuse the orginal keys in order of their disposal.
                Assert.Same(rc1, rd1);
                Assert.Same(rb2, re1);
                Assert.Same(ra1, rf1);

                // Show that the fourth key is a new semaphore.
                Assert.NotSame(rg1, rd1);
                Assert.NotSame(rg1, re1);
                Assert.NotSame(rg1, rf1);
            }

        }
    }
}

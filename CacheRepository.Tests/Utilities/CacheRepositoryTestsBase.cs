namespace CacheRepository.Tests.Utilities
{
    public abstract  class CacheRepositoryTestsBase
    {
        private readonly TestMemoryCacheRepository _memoryCacheRepository = new TestMemoryCacheRepository();

        protected TestMemoryCacheRepository MemoryCacheRepository
        {
            get { return _memoryCacheRepository; }
        }

        protected IAsyncCacheRepository CacheRepository
        {
            get { return MemoryCacheRepository; }
        }
    }
}
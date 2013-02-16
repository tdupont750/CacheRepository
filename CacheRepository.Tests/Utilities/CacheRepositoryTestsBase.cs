namespace CacheRepository.Tests.Utilities
{
    public abstract  class CacheRepositoryTestsBase
    {
        private readonly MemoryCacheRepository _memoryCacheRepository = new MemoryCacheRepository();

        protected MemoryCacheRepository MemoryCacheRepository
        {
            get { return _memoryCacheRepository; }
        }

        protected ICacheRepository CacheRepository
        {
            get { return MemoryCacheRepository; }
        }
    }
}
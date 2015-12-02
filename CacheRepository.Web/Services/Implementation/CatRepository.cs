using System;
using System.Threading.Tasks;
using CacheRepository.Web.Models;

namespace CacheRepository.Web.Services.Implementation
{
    public class CatRepository : ICatRepository
    {
        private bool _isDisposed;

        ~CatRepository()
        {
            Dispose(true);
        }

        public Task<Cat> LoadCatAsync(int id)
        {
            Cat result;

            switch (id)
            {
                case 1:
                    result = new Cat
                    {
                        Id = 1,
                        Name = "Linq"
                    };
                    break;

                case 2:
                    result = new Cat
                    {
                        Id = 1,
                        Name = "Sql"
                    };
                    break;

                default:
                    result = null;
                    break;
            }

            return Task.FromResult(result);
        }

        public void Dispose()
        {
            Dispose(false);
        }

        private void Dispose(bool isDisposing)
        {
            if (_isDisposed)
                return;

            if (!isDisposing)
                GC.SuppressFinalize(this);

            _isDisposed = true;
        }
    }
}
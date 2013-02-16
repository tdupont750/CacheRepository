using System;
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

        public Cat LoadCat(int id)
        {
            switch (id)
            {
                case 1:
                    return new Cat
                           {
                               Id = 1,
                               Name = "Linq"
                           };

                case 2:
                    return new Cat
                           {
                               Id = 1,
                               Name = "Sql"
                           };

                default:
                    return null;
            }
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
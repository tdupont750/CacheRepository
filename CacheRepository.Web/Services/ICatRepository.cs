using System;
using CacheRepository.Web.Models;

namespace CacheRepository.Web.Services
{
    public interface ICatRepository : IDisposable
    {
        Cat LoadCat(int id);
    }
}
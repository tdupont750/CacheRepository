using System;
using System.Threading.Tasks;
using CacheRepository.Web.Models;

namespace CacheRepository.Web.Services
{
    public interface ICatRepository : IDisposable
    {
        Task<Cat> LoadCatAsync(int id);
    }
}
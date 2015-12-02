using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using CacheRepository.Web.Services;

namespace CacheRepository.Web.Controllers
{
    public class CatController : Controller
    {
        private readonly IAsyncCacheRepository _cacheRepository;
        private readonly Func<ICatRepository> _catRepositoryFactory;

        public CatController(
            IAsyncCacheRepository cacheRepository, 
            Func<ICatRepository> catRepositoryFactory)
        {
            _cacheRepository = cacheRepository;
            _catRepositoryFactory = catRepositoryFactory;
        }

        public async Task<ActionResult> Get(int id)
        {
            var loadedFromCache = true;
            var cat = await _cacheRepository.GetOrSetByTypeAsync(id, async () =>
            {
                loadedFromCache = false;
                
                using (var catRepository = _catRepositoryFactory())
                    return await catRepository.LoadCatAsync(id).ConfigureAwait(false);

            }, CacheExpiration.VeryShort).ConfigureAwait(false);

            return Json(new
            {
                LoadedFromCache = loadedFromCache,
                Cat = cat
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
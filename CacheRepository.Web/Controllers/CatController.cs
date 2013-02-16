using System;
using System.Web.Mvc;
using CacheRepository.Web.Models;
using CacheRepository.Web.Services;

namespace CacheRepository.Web.Controllers
{
    public class CatController : Controller
    {
        private readonly ICacheRepository _cacheRepository;
        private readonly Func<ICatRepository> _catRepositoryFactory;

        public CatController(
            ICacheRepository cacheRepository, 
            Func<ICatRepository> catRepositoryFactory)
        {
            _cacheRepository = cacheRepository;
            _catRepositoryFactory = catRepositoryFactory;
        }

        public ActionResult Get(int id)
        {
            var loadedFromCache = true;
            var cat = _cacheRepository.GetOrSetByType(id, () =>
            {
                loadedFromCache = false;
                
                using (var catRepository = _catRepositoryFactory())
                    return catRepository.LoadCat(id);

            }, CacheExpiration.VeryShort);

            return Json(new
            {
                LoadedFromCache = loadedFromCache,
                Cat = cat
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
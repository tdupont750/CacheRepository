using System.Web.Mvc;
using CacheRepository.Web.Controllers;
using CacheRepository.Web.Services;
using CacheRepository.Web.Services.Implementation;
using Microsoft.Practices.Unity;
using Unity.Mvc3;

namespace CacheRepository.Web
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            container
                .RegisterType<ICacheRepository, WebCacheRepository>(new ContainerControlledLifetimeManager())
                .RegisterType<ICatRepository, CatRepository>(new TransientLifetimeManager());           

            return container;
        }
    }
}
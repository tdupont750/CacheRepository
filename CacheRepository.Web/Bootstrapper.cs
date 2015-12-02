using System.Web.Mvc;
using CacheRepository.Configuration;
using CacheRepository.Configuration.Implementation;
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
                .RegisterType<IAsyncCacheRepository, WebCacheRepository>(new ContainerControlledLifetimeManager())
                .RegisterType<ICacheSettings, ConfigurationCacheSettings>(new ContainerControlledLifetimeManager())
                .RegisterType<ICatRepository, CatRepository>(new TransientLifetimeManager());

            return container;
        }
    }
}
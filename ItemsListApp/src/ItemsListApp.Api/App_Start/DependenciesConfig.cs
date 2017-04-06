using System.Web.Http;
using ItemsListApp.Api.DependencyInjection;
using ItemsListApp.Contracts.DependecnyInjection;
using Microsoft.Practices.Unity;

namespace ItemsListApp.Api
{
    internal static class DependenciesConfig
    {
        public static void Register(HttpConfiguration httpConfig)
        {
            var container = new UnityContainer();

            Register<Repository.DependencyRegister>(container);
            Register<DependencyRegister>(container);
            Register<Services.DependencyRegister>(container);
        
            httpConfig.DependencyResolver = new UnityResolver(container);
        }

        private static void Register<TRegister>(IUnityContainer container)
            where TRegister : IDependencyRegister, new()
        {
            var register = new TRegister();
            register.Register(container);
        }
    }
}
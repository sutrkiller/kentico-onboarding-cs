using ItemsListApp.Contracts.DependecnyInjection;
using ItemsListApp.Contracts.Services;
using ItemsListApp.Services.Identity;
using ItemsListApp.Services.Items;
using Microsoft.Practices.Unity;

namespace ItemsListApp.Services
{
    public class DependencyRegister : IDependencyRegister
    {
        public void Register(IUnityContainer container)
        {
            container.RegisterType<IItemsService, ItemsService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IIdGeneratorService, IdGeneratorService>(new ContainerControlledLifetimeManager());
        }
    }
}

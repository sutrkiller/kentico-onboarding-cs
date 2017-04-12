using ItemsListApp.Contracts.DependecnyInjection;
using ItemsListApp.Contracts.Services;
using ItemsListApp.Services.Items;
using ItemsListApp.Services.StaticWrappers;
using Microsoft.Practices.Unity;

namespace ItemsListApp.Services
{
    public class DependencyRegister : IDependencyRegister
    {
        public void Register(IUnityContainer container)
        {
            container.RegisterType<IItemsService, ItemsService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IDateTimeService, DateTimeService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IIdentifierService, IdentifierService>(new ContainerControlledLifetimeManager());
        }
    }
}

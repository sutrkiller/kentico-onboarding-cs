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
            container.RegisterType<IItemsModificationService, ItemsModificationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IItemsCreationService, ItemsCreationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IDateTimeService, DateTimeService>(new HierarchicalLifetimeManager());
            container.RegisterType<IIdentifierService, IdentifierService>(new HierarchicalLifetimeManager());
        }
    }
}

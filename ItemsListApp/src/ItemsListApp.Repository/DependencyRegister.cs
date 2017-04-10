using ItemsListApp.Contracts.DependecnyInjection;
using ItemsListApp.Contracts.Repository;
using Microsoft.Practices.Unity;

namespace ItemsListApp.Repository
{
    public class DependencyRegister : IDependencyRegister
    {
        public void Register(IUnityContainer container)
        {
            container.RegisterType<IItemsRepository, ItemsRepository>(new ContainerControlledLifetimeManager());
        }
    }
}

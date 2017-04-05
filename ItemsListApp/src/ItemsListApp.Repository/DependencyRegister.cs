using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemsListApp.Contracts.DependecnyInjection;
using ItemsListApp.Contracts.Repository;
using Microsoft.Practices.Unity;

namespace ItemsListApp.Repository
{
    public class DependencyRegister : IDependencyRegister
    {
        public void Register(IUnityContainer container)
        {
            container.RegisterType<IItemRepository, ItemRepository>(new ContainerControlledLifetimeManager());
        }
    }
}

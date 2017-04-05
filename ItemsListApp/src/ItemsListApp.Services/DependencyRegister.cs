using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemsListApp.Contracts.DependecnyInjection;
using ItemsListApp.Contracts.Repository;
using ItemsListApp.Contracts.Services;
using ItemsListApp.Services.Items;
using Microsoft.Practices.Unity;

namespace ItemsListApp.Services
{
    public class DependencyRegister : IDependencyRegister
    {
        public void Register(IUnityContainer container)
        {
            container.RegisterType<IItemsService, ItemsService>(new ContainerControlledLifetimeManager());
        }
    }
}

﻿using System.Net.Http;
using System.Web;
using ItemsListApp.Api.Helpers;
using ItemsListApp.Contracts.Api;
using ItemsListApp.Contracts.DependecnyInjection;
using Microsoft.Practices.Unity;

namespace ItemsListApp.Api
{
    public class DependencyRegister : IDependencyRegister
    {
        public void Register(IUnityContainer container)
        {
            container.RegisterType<HttpRequestMessage>(
                new HierarchicalLifetimeManager(),
                new InjectionFactory(GetCurrentRequestMessage));
            container.RegisterType<IItemLocationHelper, ItemLocationHelper>(new HierarchicalLifetimeManager());
        }

        private static HttpRequestMessage GetCurrentRequestMessage(IUnityContainer container)
        {
            return (HttpRequestMessage)HttpContext.Current.Items["MS_HttpRequestMessage"];
        }
    }
}
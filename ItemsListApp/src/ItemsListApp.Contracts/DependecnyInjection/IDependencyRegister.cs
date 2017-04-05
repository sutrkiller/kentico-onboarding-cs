using Microsoft.Practices.Unity;

namespace ItemsListApp.Contracts.DependecnyInjection
{
    public interface IDependencyRegister
    {
        void Register(IUnityContainer container);
    }
}

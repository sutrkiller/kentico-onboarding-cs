using System.Web;
using System.Web.Http;

namespace ItemsListApp.Api
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configure(DependenciesConfig.Register);
        }
    }
}

using System.Web.Http;
using Newtonsoft.Json.Serialization;

namespace ItemsListApp.Api
{
    public static class JsonFormatterConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config
                .Formatters
                .JsonFormatter
                .SerializerSettings
                .ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
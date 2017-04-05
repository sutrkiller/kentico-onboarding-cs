using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using ItemsListApp.Api.Helpers;
using ItemsListApp.Contracts.Api;
using NSubstitute;
using NUnit.Framework;

namespace ItemsListApp.Api.UnitTests.Tests.Helpers
{
    [TestFixture]
    public class ItemLocationHelperUnitTests
    {
        private IItemLocationHelper _itemLocationHelper;
        private HttpRequestMessage _httpRequestMessage;

        [SetUp]
        public void SetUp()
        {
            _httpRequestMessage = Substitute.For<HttpRequestMessage>();
            
            _itemLocationHelper = new ItemLocationHelper(_httpRequestMessage);       
        }


        [Test]
        public void CreateLocation_Guid_StringEndingWithId()
        {
            var guid = new Guid("C648EE26-71CA-4968-9540-AD3470DA0422");
            ConfigureRequestMessage(guid);

            var location = _itemLocationHelper.CreateLocation(guid);

            Assert.That(location, Does.EndWith(guid.ToString()));
        }

        private void ConfigureRequestMessage(Guid id)
        {
            var configuration = new HttpConfiguration();
            var route = configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/v1/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            var routeData = new HttpRouteData(route,
                new HttpRouteValueDictionary
                {
                    { "id", id },
                    { "controller", "Items" }
                }
            );

            _httpRequestMessage.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, configuration);
            _httpRequestMessage.Properties.Add(HttpPropertyKeys.HttpRouteDataKey, routeData);
        }


    }
}

using System;
using System.Net.Http;
using System.Web.Http.Routing;
using ItemsListApp.Contracts.Api;

namespace ItemsListApp.Api.Helpers
{
    internal class ItemLocationHelper : IItemLocationHelper
    {
        private readonly HttpRequestMessage _requestMessage;

        public ItemLocationHelper(HttpRequestMessage requestMessage)
        {
            _requestMessage = requestMessage;
        }

        public string CreateLocation(Guid id)
        {
            var urlHelper = new UrlHelper(_requestMessage);

            return urlHelper.Route("DefaultApi", new {id});
        }
    }
}
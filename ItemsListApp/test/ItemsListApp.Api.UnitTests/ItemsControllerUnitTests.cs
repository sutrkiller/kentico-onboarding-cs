using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using ItemsListApp.Api.Controllers;
using ItemsListApp.Api.Models;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace ItemsListApp.Api.UnitTests
{
    [TestFixture]
    public class ItemsControllerUnitTests
    {
        private ItemsController _controller;

        [SetUp]
        public void SetUp()
        {
            _controller = new ItemsController
            {
                Request = new HttpRequestMessage(),
                ControllerContext = {Configuration = new HttpConfiguration()},
            };
        }

        [Test]
        public async Task TestGet()
        {
            var expected = new Item
            {
                Id = Guid.NewGuid(),
                Text = "Text of required item",
            };

            var action = await _controller.GetAsync(expected.Id);
            var response = await action.ExecuteAsync(CancellationToken.None);

            Item actual;
            response.TryGetContentValue(out actual);

            Assert.That(response.IsSuccessStatusCode);
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public async Task TestGetAll()
        {
            var expected = new []
            {
                new Item { Id = new Guid("A3672C82-AF6C-44AD-836E-D1C26A0A6359"), Text = "Dummy text 1" },
                new Item { Id = new Guid("F5CFB0AF-EB26-478B-AF41-7DA314458706"), Text = "Dummy text 2" },
                new Item { Id = new Guid("A77EE2AF-B6A2-456B-8683-A34B37B6E70F"), Text = "Dummy text 3" },
            };

            var action = await _controller.GetAsync();
            var response = await action.ExecuteAsync(CancellationToken.None);

            IEnumerable<Item> actual;
            response.TryGetContentValue(out actual);

            Assert.That(response.IsSuccessStatusCode);
            Assert.That(actual, Is.EquivalentTo(expected));
        }

        [Test]
        public async Task TestPost()
        {
            var requestUri = "http://localhost:55036/api/v1/items/";

            _controller.Request.RequestUri = new Uri(requestUri);
            var expected = new Item
            {
                Id = new Guid("97DDD880-D922-4A0D-BB07-E35339F4F5BE"),
                Text = "Something extremely creative",
            };

            var action = await _controller.PostAsync(expected.Text);
            var response = await action.ExecuteAsync(CancellationToken.None);

            Item actual;
            response.TryGetContentValue(out actual);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(actual, Is.EqualTo(expected));
            Assert.That(response.Headers.Location.ToString(), Does.EndWith(expected.Id.ToString()));

        }

        [Test]
        public async Task TestPut()
        {
            var expected = new Item
            {
                Id = Guid.NewGuid(),
                Text = "Text of required item",
            };

            var action = await _controller.PutAsync(expected);
            var response = await action.ExecuteAsync(CancellationToken.None);

            Item actual;
            response.TryGetContentValue(out actual);

            Assert.That(response.IsSuccessStatusCode);
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public async Task TestDelete()
        {
            var id = Guid.NewGuid();

            var action = await _controller.DeleteAsync(id);
            var response = await action.ExecuteAsync(CancellationToken.None);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }
    }
}

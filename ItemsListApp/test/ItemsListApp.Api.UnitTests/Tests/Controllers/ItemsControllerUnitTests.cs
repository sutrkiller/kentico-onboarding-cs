using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using ItemsListApp.Api.Controllers;
using ItemsListApp.Contracts.Api;
using ItemsListApp.Contracts.Models;
using ItemsListApp.Contracts.Repository;
using ItemsListApp.Contracts.Services;
using ItemsListApp.Contracts.UnitTests.Base.Helpers;
using NSubstitute;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace ItemsListApp.Api.UnitTests.Tests.Controllers
{
    [TestFixture]
    public class ItemsControllerUnitTests
    {
        private ItemsController _itemsController;

        private IItemsRepository _itemsRepository;
        private IItemsService _itemsService;
        private IItemLocationHelper _itemLocationHelper;

        [SetUp]
        public void SetUp()
        {
            _itemsRepository = Substitute.For<IItemsRepository>();
            _itemsService = Substitute.For<IItemsService>();
            _itemLocationHelper = Substitute.For<IItemLocationHelper>();

            _itemsController = new ItemsController(_itemsRepository, _itemsService, _itemLocationHelper)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration(),
            };
        }

        [Test]
        public async Task Get_IdOfItem_ReturnsItemWithThisId()
        {
            var itemId = new Guid("6341EB90-93E6-49AA-BBEC-2A69C3C8DB8D");
            var expected = new Item
            {
                Id = itemId,
                Text = "Text of required item",
            };
            _itemsRepository.GetByIdAsync(itemId).Returns(expected);

            var action = await _itemsController.GetAsync(expected.Id);
            var response = await action.ExecuteAsync(CancellationToken.None);
            Item actual;
            response.TryGetContentValue(out actual);

            Assert.That(response.IsSuccessStatusCode);
            Assert.That(actual, Is.EqualTo(expected).UsingItemComparer());
        }

        [Test]
        public async Task Get_NoParameters_ReturnsAllItems()
        {
            var expected = new[]
            {
                new Item {Id = new Guid("A3672C82-AF6C-44AD-836E-D1C26A0A6359"), Text = "Dummy text 1"},
                new Item {Id = new Guid("F5CFB0AF-EB26-478B-AF41-7DA314458706"), Text = "Dummy text 2"},
                new Item {Id = new Guid("A77EE2AF-B6A2-456B-8683-A34B37B6E70F"), Text = "Dummy text 3"},
            };
            _itemsRepository.GetAllAsync().Returns(expected.AsQueryable());


            var action = await _itemsController.GetAsync();
            var response = await action.ExecuteAsync(CancellationToken.None);
            IEnumerable<Item> actual;
            response.TryGetContentValue(out actual);

            Assert.That(response.IsSuccessStatusCode);
            Assert.That(actual, Is.EqualTo(expected).AsCollection.UsingItemComparer());
        }

        [Test]
        public async Task Post_ValidText_ReturnsCreatedItem()
        {
            var itemId = new Guid("97DDD880-D922-4A0D-BB07-E35339F4F5BE");
            var expected = new Item
            {
                Id = itemId,
                Text = "Something extremely creative",
            };
            _itemLocationHelper.CreateLocation(itemId).Returns($"dummy location/{itemId}");
            _itemsService.AddItemAsync(expected.Text).Returns(expected);

            var action = await _itemsController.PostAsync(expected.Text);
            var response = await action.ExecuteAsync(CancellationToken.None);
            Item actual;
            response.TryGetContentValue(out actual);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(actual, Is.EqualTo(expected).UsingItemComparer());
            Assert.That(response.Headers.Location.ToString(), Does.EndWith(expected.Id.ToString()).IgnoreCase);
        }

        [Test]
        public async Task Put_ItemWithId_ReturnsSameItem()
        {
            var expected = new Item
            {
                Id = new Guid("999EA6F0-4139-4D54-B4DD-4976A35D1DFA"),
                Text = "Text of required item",
            };

            var action = await _itemsController.PutAsync(expected);
            var response = await action.ExecuteAsync(CancellationToken.None);
            Item actual;
            response.TryGetContentValue(out actual);

            Assert.That(response.IsSuccessStatusCode);
            Assert.That(actual, Is.EqualTo(expected).UsingItemComparer());
        }

        [Test]
        public async Task Delete_IdOfExistingItem_ReturnsNoContentStatusCode()
        {
            var id = Guid.NewGuid();

            var action = await _itemsController.DeleteAsync(id);
            var response = await action.ExecuteAsync(CancellationToken.None);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }
    }
}
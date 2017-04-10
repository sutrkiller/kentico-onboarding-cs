﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using ItemsListApp.Api.Controllers;
using ItemsListApp.Api.UnitTests.Helpers;
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

        private IItemsService _itemsService;
        private IItemLocationHelper _itemLocationHelper;
        private IItemsRepository _itemsRepository;

        [SetUp]
        public void SetUp()
        {
            _itemsService = Substitute.For<IItemsService>();
            _itemLocationHelper = Substitute.For<IItemLocationHelper>();
            _itemsRepository = Substitute.For<IItemsRepository>();

            _itemsController = new ItemsController(_itemsService, _itemsRepository, _itemLocationHelper)
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
                Text = "InvalidateText of required item",
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
        public async Task Get_NonExistentId_ReturnsNull()
        {
            var itemId = new Guid("6341EB90-93E6-49AA-BBEC-2A69C3C8DB8D");
            _itemsRepository.GetByIdAsync(itemId).Returns((Item) null);

            var action = await _itemsController.GetAsync(itemId);
            var response = await action.ExecuteAsync(CancellationToken.None);
            Item actual;
            response.TryGetContentValue(out actual);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(actual, Is.Null);
        }

        [Test]
        public async Task Get_InvalidId_ReturnsBadRequest()
        {
            var invalidId = Guid.Empty;

            var action = await _itemsController.GetAsync(invalidId);
            var response = await action.ExecuteAsync(CancellationToken.None);
            HttpError actual;
            response.TryGetContentValue(out actual);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(actual.Keys.Count, Is.GreaterThan(0));
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
            _itemsRepository.GetAllAsync().Returns(expected);


            var action = await _itemsController.GetAsync();
            var response = await action.ExecuteAsync(CancellationToken.None);
            IEnumerable<Item> actual;
            response.TryGetContentValue(out actual);

            Assert.That(response.IsSuccessStatusCode);
            Assert.That(actual, Is.EqualTo(expected).AsCollection.UsingItemComparer());
        }

        [Test]
        public async Task Post_ValidItem_ReturnsCreatedItem()
        {
            var itemId = new Guid("97DDD880-D922-4A0D-BB07-E35339F4F5BE");
            var postItem = new Item
            {
                Text = "Something extremely creative",
            };
            var expected = new Item
            {
                Id = itemId,
                Text = postItem.Text,
            };
            _itemLocationHelper.CreateLocation(itemId).Returns($"dummy location/{itemId}");
            _itemsService.CreateNewAsync(postItem).Returns(expected);

            var action = await _itemsController.PostAsync(postItem);
            var response = await action.ExecuteAsync(CancellationToken.None);
            Item actual;
            response.TryGetContentValue(out actual);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(actual, Is.EqualTo(expected).UsingItemComparer());
            Assert.That(response.Headers.Location.ToString(), Does.EndWith(itemId.ToString()).IgnoreCase);
        }

        [Test, TestCaseSource(typeof(InvalidPostTestCases))]
        public async Task Post_InvalidItem_ReturnsBadRequest(Item postItem, IEnumerable<string> modelStateErrorKeys)
        {
            var action = await _itemsController.PostAsync(postItem);
            var response = await action.ExecuteAsync(CancellationToken.None);
            HttpError actual;
            response.TryGetContentValue(out actual);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(actual.ModelState.Keys, Is.EquivalentTo(modelStateErrorKeys).IgnoreCase);
        }

        [Test]
        public async Task Put_ItemWithId_ReturnsSuccessCodeRequest()
        {
            var expected = new Item
            {
                Id = new Guid("999EA6F0-4139-4D54-B4DD-4976A35D1DFA"),
                Text = "InvalidateText of required item",
            };
            _itemsService.ExistsAsync(expected.Id).Returns(true);
            _itemsService.ReplaceExistingAsync(expected).Returns(expected);

            var action = await _itemsController.PutAsync(expected);
            var response = await action.ExecuteAsync(CancellationToken.None);
            Item actual;
            response.TryGetContentValue(out actual);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(actual, Is.EqualTo(expected).UsingItemComparer());
        }

        [Test]
        public async Task Put_ItemWithNonExistingId_ReturnNewItem()
        {
            var putItem = new Item
            {
                Id = new Guid("3C4A53C3-C24F-4755-B871-9F2059A09F74"),
                Text = "InvalidateText of required item",
            };
            var expected = new Item
            {
                Id = new Guid("999EA6F0-4139-4D54-B4DD-4976A35D1DFA"),
                Text = putItem.Text,
            };
            _itemsService.ExistsAsync(putItem.Id).Returns(false);
            _itemsService.CreateNewAsync(putItem).Returns(expected);

            var action = await _itemsController.PutAsync(putItem);
            var response = await action.ExecuteAsync(CancellationToken.None);
            Item actual;
            response.TryGetContentValue(out actual);

            Assert.That(response.StatusCode,Is.EqualTo(HttpStatusCode.Created));
            Assert.That(actual, Is.EqualTo(expected).UsingItemComparer());
        }

        [Test, TestCaseSource(typeof(InvalidPutTestCases))]
        public async Task Put_InvalidItem_ReturnsBadRequest(Item postItem, IEnumerable<string> modelStateErrorKeys)
        {
            var action = await _itemsController.PutAsync(postItem);
            var response = await action.ExecuteAsync(CancellationToken.None);
            HttpError actual;
            response.TryGetContentValue(out actual);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(actual.ModelState.Keys, Is.EquivalentTo(modelStateErrorKeys).IgnoreCase);
        }

        [Test]
        public async Task Delete_IdOfExistingItem_ReturnsNoContentStatusCode()
        {
            var id = Guid.NewGuid();

            var action = await _itemsController.DeleteAsync(id);
            var response = await action.ExecuteAsync(CancellationToken.None);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        private class InvalidPostTestCases : IEnumerable<TestCaseData>
        {
            public IEnumerator<TestCaseData> GetEnumerator()
            {
                yield return new ItemPostTestCaseBuilder()
                    .InvalidateId(new Guid("999EA6F0-4139-4D54-B4DD-4976A35D1DFA"))
                    .Build();

                yield return new ItemPostTestCaseBuilder()
                    .InvalidateText(string.Empty)
                    .Build();

                yield return new ItemPostTestCaseBuilder()
                    .InvalidateText("   ")
                    .Build();

                yield return new ItemPostTestCaseBuilder()
                    .InvalidateText(null)
                    .Build();

                yield return new ItemPostTestCaseBuilder()
                    .InvalidateCreationTime(
                        new DateTime(year: 2017, month: 4, day: 6, hour: 12, minute: 12, second: 50))
                    .Build();

                yield return new ItemPostTestCaseBuilder()
                    .InvalidateLastUpdateTime(
                        new DateTime(year: 2017, month: 4, day: 6, hour: 12, minute: 12, second: 50))
                    .Build();

                yield return new ItemPostTestCaseBuilder()
                    .InvalidateId(new Guid("999EA6F0-4139-4D54-B4DD-4976A35D1DFA"))
                    .InvalidateText(string.Empty)
                    .InvalidateCreationTime(
                        new DateTime(year: 2017, month: 2, day: 4, hour: 2, minute: 8, second: 17))
                    .InvalidateLastUpdateTime(
                        new DateTime(year: 2017, month: 4, day: 6, hour: 12, minute: 12, second: 50))
                    .Build();

                yield return new ItemPostTestCaseBuilder()
                    .InvalidItem();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private class InvalidPutTestCases : IEnumerable<TestCaseData>
        {
            public IEnumerator<TestCaseData> GetEnumerator()
            {
                yield return new ItemPutTestCaseBuilder()
                    .InvalidateId(Guid.Empty)
                    .Build();

                yield return new ItemPutTestCaseBuilder()
                    .InvalidateText(string.Empty)
                    .Build();

                yield return new ItemPutTestCaseBuilder()
                    .InvalidateText("   ")
                    .Build();

                yield return new ItemPutTestCaseBuilder()
                    .InvalidateText(null)
                    .Build();

                yield return new ItemPutTestCaseBuilder()
                    .InvalidateCreationTime(
                        new DateTime(year: 2017, month: 4, day: 6, hour: 12, minute: 12, second: 50))
                    .Build();

                yield return new ItemPutTestCaseBuilder()
                    .InvalidateLastUpdateTime(
                        new DateTime(year: 2017, month: 4, day: 6, hour: 12, minute: 12, second: 50))
                    .Build();

                yield return new ItemPutTestCaseBuilder()
                    .InvalidateId(Guid.Empty)
                    .InvalidateText(string.Empty)
                    .InvalidateCreationTime(
                        new DateTime(year: 2017, month: 2, day: 4, hour: 2, minute: 8, second: 17))
                    .InvalidateLastUpdateTime(
                        new DateTime(year: 2017, month: 4, day: 6, hour: 12, minute: 12, second: 50))
                    .Build();

                yield return new ItemPutTestCaseBuilder()
                    .InvalidItem();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
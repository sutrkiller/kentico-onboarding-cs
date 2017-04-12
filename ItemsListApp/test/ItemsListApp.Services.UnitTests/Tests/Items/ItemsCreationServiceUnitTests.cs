using System;
using System.Threading.Tasks;
using ItemsListApp.Contracts.Models;
using ItemsListApp.Contracts.Repository;
using ItemsListApp.Contracts.Services;
using ItemsListApp.Contracts.UnitTests.Base.Helpers;
using ItemsListApp.Services.Items;
using NSubstitute;
using NUnit.Framework;

namespace ItemsListApp.Services.UnitTests.Tests.Items
{
    [TestFixture]
    public class ItemsCreationServiceUnitTests
    {
        private ItemsCreationService _itemsCreationService;
        private IItemsRepository _itemsRepository;
        private IDateTimeService _dateTimeService;
        private IIdentifierService _identifierService;

        [SetUp]
        public void SetUp()
        {
            _itemsRepository = Substitute.For<IItemsRepository>();
            _dateTimeService = Substitute.For<IDateTimeService>();
            _identifierService = Substitute.For<IIdentifierService>();
            _itemsCreationService = new ItemsCreationService(_itemsRepository, _identifierService, _dateTimeService);
        }

        [Test]
        public async Task CreateNewAsync_ValidText_ReturnsNewItemWithIdAndCorrectProperties()
        {
            var creationTime = new DateTime(year: 2017, month: 4, day: 6, hour: 12, minute: 12, second: 50);
            var postItem = new Item
            {
                Text = "text of new item"
            };
            var expectedItem = new Item
            {
                Id = new Guid("95AB19B6-455B-469C-83AA-CD505E9389BD"),
                Text = postItem.Text,
                CreationTime = creationTime,
                LastUpdateTime = creationTime,
            };
            Item storedItem = null;
            _identifierService.GenerateIdAsync().Returns(expectedItem.Id);
            _itemsRepository.AddAsync(Arg.Do<Item>(item => { storedItem = item; })).Returns(Task.CompletedTask);
            _dateTimeService.GetCurrentDateAsync().Returns(creationTime);

            var newItem = await _itemsCreationService.CreateNewAsync(postItem);

            Assert.That(storedItem, Is.EqualTo(expectedItem).UsingItemComparer());
            Assert.That(newItem, Is.EqualTo(expectedItem).UsingItemComparer());
        }

        [Test]
        public async Task CreateNewAsync_ItemAndValidId_ReturnsNewItemWithOriginalId()
        {
            var creationTime = new DateTime(year: 2017, month: 4, day: 6, hour: 12, minute: 12, second: 50);
            var postItem = new Item
            {
                Id = new Guid("95AB19B6-455B-469C-83AA-CD505E9389BD"),
                Text = "text of new item"
            };
            var expectedItem = new Item
            {
                Id = postItem.Id,
                Text = postItem.Text,
                CreationTime = creationTime,
                LastUpdateTime = creationTime,
            };
            Item storedItem = null;
            _itemsRepository.AddAsync(Arg.Do<Item>(item => { storedItem = item; })).Returns(Task.CompletedTask);
            _dateTimeService.GetCurrentDateAsync().Returns(creationTime);

            var newItem = await _itemsCreationService.CreateNewAsync(postItem, postItem.Id);

            Assert.That(storedItem, Is.EqualTo(expectedItem).UsingItemComparer());
            Assert.That(newItem, Is.EqualTo(expectedItem).UsingItemComparer());
        }
    }
}
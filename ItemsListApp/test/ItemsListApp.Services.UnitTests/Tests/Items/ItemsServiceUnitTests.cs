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
    public class ItemsServiceUnitTests
    {
        private ItemsService _itemsService;
        private IItemsRepository _itemsRepository;
        private IIdGeneratorService _idGeneratorService;
        private IDateTimeService _dateTimeService;

        [SetUp]
        public void SetUp()
        {
            _itemsRepository = Substitute.For<IItemsRepository>();
            _idGeneratorService = Substitute.For<IIdGeneratorService>();
            _dateTimeService = Substitute.For<IDateTimeService>();
            _itemsService = new ItemsService(_itemsRepository, _idGeneratorService, _dateTimeService);
        }

        [Test]
        public async Task AddItemAsync_ValidText_ReturnsNewItemWithIdAndCorrectProperties()
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
            _itemsRepository.AddAsync(Arg.Do<Item>(item => { storedItem = item; })).Returns(Task.CompletedTask);
            _idGeneratorService.GenerateIdAsync().Returns(expectedItem.Id);
            _dateTimeService.GetCurrentDateAsync().Returns(creationTime);

            var newItem = await _itemsService.AddItemAsync(postItem);

            Assert.That(storedItem, Is.EqualTo(expectedItem).UsingItemComparer());
            Assert.That(newItem, Is.EqualTo(expectedItem).UsingItemComparer());
        }

        [Test]
        public async Task GetItemByIdAsync_ExistingId_ReturnsItemWithThisId()
        {
            var creationTime = new DateTime(year: 2017, month: 4, day: 6, hour: 12, minute: 12, second: 50);
            var expected = new Item
            {
                Id = new Guid("95AB19B6-455B-469C-83AA-CD505E9389BD"),
                Text = "cool text",
                CreationTime = creationTime,
                LastUpdateTime = creationTime,
            };
            _itemsRepository.GetByIdAsync(expected.Id).Returns(expected);

            var item = await _itemsService.GetByIdAsync(expected.Id);

            Assert.That(item, Is.EqualTo(expected).UsingItemComparer());
        }

        [Test]
        public async Task GetItemByIdAsync_NonExistingId_ReturnsNull()
        {
            var id = new Guid("95AB19B6-455B-469C-83AA-CD505E9389BD");
            _itemsRepository.GetByIdAsync(id).Returns((Item) null);

            var item = await _itemsService.GetByIdAsync(id);

            Assert.That(item, Is.Null);
        }

        [Test]
        public async Task GetAllAsync_ReturnsCollectionOfItems()
        {
            var expected = new[]
            {
                new Item
                {
                    Id = new Guid("95AB19B6-455B-469C-83AA-CD505E9389BD"),
                    Text = "cool text",
                },
                new Item
                {
                    Id = new Guid("95AB19B6-455B-469C-83AA-CD505E9389ce"),
                    Text = "cool text 2",
                },
            };
            _itemsRepository.GetAllAsync().Returns(expected);

            var items = await _itemsService.GetAllAsync();

            Assert.That(items, Is.EqualTo(expected).AsCollection.UsingItemComparer());
        }

        [Test]
        public async Task PutAsync_ValidItem_ReturnsSameItemWithUpdatedTime()
        {
            var creationTime = new DateTime(year: 2017, month: 4, day: 6, hour: 12, minute: 12, second: 50);
            var updateTime = new DateTime(year: 2017, month: 5 , day: 3, hour: 12, minute: 12, second: 1);
            var putItem = new Item
            {
                Id = new Guid("95AB19B6-455B-469C-83AA-CD505E9389BD"),
                Text = "cool text",
            };
            var repositoryItem = new Item
            {
                Id = putItem.Id,
                Text = "other text",
                CreationTime = creationTime,
                LastUpdateTime = creationTime,
            };
            var expected = new Item
            {
                Id = putItem.Id,
                Text = putItem.Text,
                CreationTime = creationTime,
                LastUpdateTime = updateTime,
            };
            _dateTimeService.GetCurrentDateAsync().Returns(updateTime);
            _itemsRepository.GetByIdAsync(putItem.Id).Returns(repositoryItem);

            var item = await _itemsService.PutAsync(putItem);

            Assert.That(item, Is.EqualTo(expected).UsingItemComparer());
        }

        [Test]
        public async Task PutAsync_InvalidItem_ReturnsNull()
        {
            var expected = new Item
            {
                Id = new Guid("95AB19B6-455B-469C-83AA-CD505E9389BD"),
                Text = "cool text",
            };
            _itemsRepository.GetByIdAsync(expected.Id).Returns(null as Item);

            var item = await _itemsService.PutAsync(expected);

            Assert.That(item, Is.Null);
        }

        [Test]
        public async Task RemoveByIdAsync_ValidId_ReturnsItemWithThisId()
        {
            var expected = new Item
            {
                Id = new Guid("95AB19B6-455B-469C-83AA-CD505E9389BD"),
                Text = "cool text",
            };
            _itemsRepository.RemoveByIdAsync(expected.Id).Returns(expected);

            var item = await _itemsService.RemoveByIdAsync(expected.Id);

            Assert.That(item, Is.EqualTo(expected).UsingItemComparer());
        }

        [Test]
        public async Task RemoveByIdAsync_InvalidId_ReturnsNull()
        {
            var id = new Guid("95AB19B6-455B-469C-83AA-CD505E9389BD");
            _itemsRepository.RemoveByIdAsync(id).Returns(null as Item);

            var item = await _itemsService.RemoveByIdAsync(id);

            Assert.That(item, Is.Null);
        }
    }
}
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

        [SetUp]
        public void SetUp()
        {
            _itemsRepository = Substitute.For<IItemsRepository>();
            _idGeneratorService = Substitute.For<IIdGeneratorService>();
            _itemsService = new ItemsService(_itemsRepository, _idGeneratorService);
        }

        [Test]
        public async Task AddItemAsync_ValidText_ReturnsNewItemWithIdAndCorrectProperties()
        {
            var id = Guid.NewGuid();
            var text = "text of new item";
            var expectedItem = new Item
            {
                Id = id,
                Text = text,
            };
            Item storedItem = null;
            _idGeneratorService.GenerateIdAsync().Returns(id);
            _itemsRepository.AddAsync(Arg.Do<Item>(item => { storedItem = item; })).Returns(Task.CompletedTask);

            var newItem = await _itemsService.AddItemAsync(expectedItem);

            Assert.That(storedItem, Is.EqualTo(expectedItem).UsingItemComparer());
            Assert.That(newItem, Is.EqualTo(expectedItem).UsingItemComparer());
        }

        [Test]
        public async Task GetItemByIdAsync_ExistingId_ReturnsItemWithThisId()
        {
            var expected = new Item
            {
                Id = new Guid("95AB19B6-455B-469C-83AA-CD505E9389BD"),
                Text = "cool text",
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
        public async Task PutAsync_ValidItem_ReturnsSameItem()
        {
            var expected = new Item
            {
                Id = new Guid("95AB19B6-455B-469C-83AA-CD505E9389BD"),
                Text = "cool text",
            };
            _itemsRepository.UpdateAsync(expected).Returns(expected);

            var item = await _itemsService.PutAsync(expected);

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
            _itemsRepository.UpdateAsync(expected).Returns((Item) null);

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
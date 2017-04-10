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
        private IIdentifierService _identifierService;

        [SetUp]
        public void SetUp()
        {
            _itemsRepository = Substitute.For<IItemsRepository>();
            _identifierService = Substitute.For<IIdentifierService>();
            _itemsService = new ItemsService(_itemsRepository, _identifierService);
        }

        [Test]
        public async Task AddItemAsync_validText_returnsNewItemWithIdAndCorrectProperties()
        {
            var id = Guid.NewGuid();
            var text = "text of new item";
            var expectedItem = new Item
            {
                Id = id,
                Text = text,
            };
            Item storedItem = null;
            _identifierService.GenerateIdAsync().Returns(id);
            _itemsRepository.AddAsync(Arg.Do<Item>(item => { storedItem = item; })).Returns(Task.CompletedTask);

            var newItem = await _itemsService.AddItemAsync(expectedItem);

            Assert.That(storedItem, Is.EqualTo(expectedItem).UsingItemComparer());
            Assert.That(newItem, Is.EqualTo(expectedItem).UsingItemComparer());
        }
    }
}
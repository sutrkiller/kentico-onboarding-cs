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
        public async Task AddItemAsync_validText_returnsNewItemWithIdAndCorrectProperties()
        {
            var id = Guid.NewGuid();
            var text = "text of new item";
            var expectedItem = new Item
            {
                Id = id,
                Text = text,
            };
            _idGeneratorService.GenerateIdAsync().Returns(id);

            var newItem = await _itemsService.AddItemAsync(text);

            Assert.That(newItem, Is.EqualTo(expectedItem).UsingItemComparer());
        }

        [Test]
        public void AddItemAsync_null_throwsArgumentNullExcepction()
        {
            Assert.That(async () => await _itemsService.AddItemAsync(null),
                Throws.ArgumentNullException
                .With.Property("ParamName")
                .EqualTo("text"));
        }

        [Test]
        public void AddItemAsync_emptyString_throwsArgumentExcepction()
        {
            Assert.That(async () => await _itemsService.AddItemAsync(""),
                Throws.ArgumentException
                    .With.Property("ParamName")
                    .EqualTo("text"));
        }

        [Test]
        public void AddItemAsync_whiteSpaceString_throwsArgumentExcepction()
        {
            Assert.That(async () => await _itemsService.AddItemAsync("   "),
                Throws.ArgumentException
                    .With.Property("ParamName")
                    .EqualTo("text"));
        }
    }
}

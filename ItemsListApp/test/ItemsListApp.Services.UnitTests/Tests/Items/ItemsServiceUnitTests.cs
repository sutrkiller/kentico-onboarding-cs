using System;
using System.Threading.Tasks;
using ItemsListApp.Contracts.Repository;
using ItemsListApp.Services.Items;
using NSubstitute;
using NUnit.Framework;

namespace ItemsListApp.Services.UnitTests.Tests.Items
{
    [TestFixture]
    public class ItemsServiceUnitTests
    {
        private ItemsService _itemsService;

        [SetUp]
        public void SetUp()
        {
            var itemsRepository = Substitute.For<IItemsRepository>();
            _itemsService = new ItemsService(itemsRepository);
        }

        [Test]
        public async Task AddItemAsync_validText_returnsNewItemWithIdAndCorrectProperties()
        {
            var text = "text of new item";

            var newItem = await _itemsService.AddItemAsync(text);

            Assert.That(newItem, Is.Not.Null);
            Assert.That(newItem.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(newItem.Text, Is.EqualTo(text));
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
                Throws.ArgumentNullException
                    .With.Property("ParamName")
                    .EqualTo("text"));
        }
    }
}

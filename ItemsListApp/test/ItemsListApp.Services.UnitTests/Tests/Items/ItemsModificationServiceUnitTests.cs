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
    public class ItemsModificationServiceUnitTests
    {
        private ItemsModificationService _itemsModificationService;
        private IItemsRepository _itemsRepository;
        private IDateTimeService _dateTimeService;

        [SetUp]
        public void SetUp()
        {
            _itemsRepository = Substitute.For<IItemsRepository>();
            _dateTimeService = Substitute.For<IDateTimeService>();
            _itemsModificationService = new ItemsModificationService(_itemsRepository, _dateTimeService);
        }

        [Test]
        public async Task ReplaceAsync_ValidItem_ReturnsSameItemWithUpdatedTime()
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

            var item = await _itemsModificationService.ReplaceAsync(putItem);

            Assert.That(item, Is.EqualTo(expected).UsingItemComparer());
        }

        [Test]
        public async Task ReplaceAsync_ValidItemIsCached_UsesCachedItem()
        {
            var putItem = new Item
            {
                Id = new Guid("95AB19B6-455B-469C-83AA-CD505E9389BD"),
            };
            _itemsRepository.GetByIdAsync(putItem.Id).Returns(putItem, (Item)null);

            await _itemsModificationService.DoesExistAsync(putItem.Id);
            var item = await _itemsModificationService.ReplaceAsync(putItem);

            Assert.That(item.Id, Is.EqualTo(putItem.Id).UsingItemComparer());
        }

        [Test]
        public async Task ReplaceAsync_ValidOtherItemIsCached_DoesNotUseCachedItem()
        {
            var putItem = new Item
            {
                Id = new Guid("95AB19B6-455B-469C-83AA-CD505E9389BD"),
            };
            var expiredItem = new Item
            {
                Id = new Guid("C026110E-7678-4FB5-9C51-B4FAA546BA1B"),
            };
            _itemsRepository.GetByIdAsync(Arg.Any<Guid>()).Returns(expiredItem, putItem);

            await _itemsModificationService.DoesExistAsync(putItem.Id);
            var item = await _itemsModificationService.ReplaceAsync(putItem);

            Assert.That(item.Id, Is.EqualTo(putItem.Id).UsingItemComparer());
        }

        [Test]
        public async Task DoesExistAsync_ExistingId_ReturnsTrue()
        {
            var repositoryItem = new Item
            {
                Id = new Guid("95AB19B6-455B-469C-83AA-CD505E9389BD"),
                Text = "other text",
            };
            _itemsRepository.GetByIdAsync(repositoryItem.Id).Returns(repositoryItem);

            var exists = await _itemsModificationService.DoesExistAsync(repositoryItem.Id);

            Assert.That(exists);
        }

        [Test]
        public async Task DoesExistAsync_NonExistingId_ReturnsTrue()
        {
            var id = new Guid("95AB19B6-455B-469C-83AA-CD505E9389BD");
            _itemsRepository.GetByIdAsync(id).Returns((Item)null);

            var exists = await _itemsModificationService.DoesExistAsync(id);

            Assert.That(!exists);
        }
    }
}
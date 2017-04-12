using System;
using System.Threading.Tasks;
using ItemsListApp.Contracts.Models;
using ItemsListApp.Contracts.Repository;
using ItemsListApp.Contracts.Services;

namespace ItemsListApp.Services.Items
{
    internal class ItemsModificationService : IItemsModificationService
    {
        private readonly IItemsRepository _itemsRepository;
        private readonly IDateTimeService _dateTimeService;

        private Item _cachedItem;

        private Item PopCachedItem(Guid id)
        {
            var poppedItem = _cachedItem?.Id == id ? _cachedItem : null;
            _cachedItem = null;
            return poppedItem;
        }

        public ItemsModificationService(IItemsRepository itemsRepository, IDateTimeService dateTimeService)
        {
            _itemsRepository = itemsRepository;
            _dateTimeService = dateTimeService;
        }

        public async Task<Item> ReplaceAsync(Item item)
        {
            var original = PopCachedItem(item.Id) ?? await _itemsRepository.GetByIdAsync(item.Id);

            if (original == null)
            {
                throw new InvalidOperationException("Item must exist");
            }

            original.Text = item.Text;
            original.LastUpdateTime = await _dateTimeService.GetCurrentDateAsync();

            await _itemsRepository.UpdateAsync(original);

            return original;
        }

        public async Task<bool> DoesExistAsync(Guid id)
        {
            _cachedItem = await _itemsRepository.GetByIdAsync(id);
            return _cachedItem != null;
        }
    }
}
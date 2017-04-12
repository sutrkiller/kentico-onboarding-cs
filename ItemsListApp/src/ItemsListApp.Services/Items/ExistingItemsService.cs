using System;
using System.Threading.Tasks;
using ItemsListApp.Contracts.Models;
using ItemsListApp.Contracts.Repository;
using ItemsListApp.Contracts.Services;

namespace ItemsListApp.Services.Items
{
    internal class ExistingItemsService : IExistingItemsService
    {
        private readonly IItemsRepository _itemsRepository;
        private readonly IDateTimeService _dateTimeService;

        private Item _cachedItem;


        public ExistingItemsService(IItemsRepository itemsRepository, IDateTimeService dateTimeService)
        {
            _itemsRepository = itemsRepository;
            _dateTimeService = dateTimeService;
        }

        public async Task<Item> ReplaceAsync(Item item)
        {
            var original = _cachedItem ?? await _itemsRepository.GetByIdAsync(item.Id);
            _cachedItem = null;

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
            => (_cachedItem = await _itemsRepository.GetByIdAsync(id)) != null;
    }
}
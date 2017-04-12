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

        public ExistingItemsService(IItemsRepository itemsRepository, IDateTimeService dateTimeService)
        {
            _itemsRepository = itemsRepository;
            _dateTimeService = dateTimeService;
        }

        public async Task<Item> ReplaceAsync(Item item)
        {
            var original = await _itemsRepository.GetByIdAsync(item.Id);
            if (original == null)
            {
                throw new InvalidOperationException("Item must exist");
            }

            original.Text = item.Text;
            original.LastUpdateTime = await _dateTimeService.GetCurrentDateAsync();

            await _itemsRepository.UpdateAsync(original);

            return original;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            var item = await _itemsRepository.GetByIdAsync(id);
            return item != null;
        }
    }
}
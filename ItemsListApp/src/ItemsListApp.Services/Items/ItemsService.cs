using System;
using System.Threading.Tasks;
using ItemsListApp.Contracts.Models;
using ItemsListApp.Contracts.Repository;
using ItemsListApp.Contracts.Services;

namespace ItemsListApp.Services.Items
{
    internal class ItemsService : IItemsService
    {
        private readonly IItemsRepository _itemsRepository;
        private readonly IIdGeneratorService _idGeneratorService;
        private readonly IDateTimeService _dateTimeService;

        public ItemsService(IItemsRepository itemsRepository, IIdGeneratorService idGeneratorService,
            IDateTimeService dateTimeService)
        {
            _itemsRepository = itemsRepository;
            _idGeneratorService = idGeneratorService;
            _dateTimeService = dateTimeService;
        }

        public async Task<Item> CreateNewAsync(Item item)
            => await CreateNewAsync(item, await _idGeneratorService.GenerateIdAsync());

        public async Task<Item> ReplaceExistingAsync(Item item)
        {
            var original = await _itemsRepository.GetByIdAsync(item.Id);
            if (original == null)
            {
                //should never happen
                return await CreateNewAsync(item, item.Id);
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

        private async Task<Item> CreateNewAsync(Item item, Guid id)
        {
            var creationTime = await _dateTimeService.GetCurrentDateAsync();
            var newItem = new Item
            {
                Id = id,
                Text = item.Text,
                CreationTime = creationTime,
                LastUpdateTime = creationTime,
            };

            await _itemsRepository.AddAsync(newItem);
            return newItem;
        }
    }
}
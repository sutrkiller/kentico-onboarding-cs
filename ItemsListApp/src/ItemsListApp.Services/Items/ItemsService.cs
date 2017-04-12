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
        private readonly IDateTimeService _dateTimeService;
        private readonly IIdentifierService _identifierService;

        public ItemsService(IItemsRepository itemsRepository, IIdentifierService identifierService,
            IDateTimeService dateTimeService)
        {
            _itemsRepository = itemsRepository;
            _identifierService = identifierService;
            _dateTimeService = dateTimeService;
        }

        public async Task<Item> CreateNewAsync(Item item)
            => await CreateNewAsync(item,
                item.Id != default(Guid) 
                ? item.Id 
                : await _identifierService.GenerateIdAsync());

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
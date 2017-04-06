using System;
using System.Collections.Generic;
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

        public ItemsService(IItemsRepository itemsRepository, IIdGeneratorService idGeneratorService, IDateTimeService dateTimeService)
        {
            _itemsRepository = itemsRepository;
            _idGeneratorService = idGeneratorService;
            _dateTimeService = dateTimeService;
        }
        public async Task<Item> AddItemAsync(Item item)
        {
            var creationTime = await _dateTimeService.GetCurrentDateAsync();
            var newItem = new Item
            {
                Id = await _idGeneratorService.GenerateIdAsync(), 
                Text = item.Text,
                CreationTime = creationTime,
                LastUpdateTime = creationTime,
            };

            await _itemsRepository.AddAsync(newItem);
            return newItem;
        }

        public async Task<Item> GetByIdAsync(Guid id)
            => await _itemsRepository.GetByIdAsync(id);

        public async Task<IEnumerable<Item>> GetAllAsync() 
            => await _itemsRepository.GetAllAsync();

        public async Task<Item> PutAsync(Item item)
        {
            var original = await _itemsRepository.GetByIdAsync(item.Id);
            if (original == null) return null;

            original.Text = item.Text;
            original.LastUpdateTime = await _dateTimeService.GetCurrentDateAsync();

            await _itemsRepository.UpdateAsync(original);

            return original;
        }

        public async Task<Item> RemoveByIdAsync(Guid id)
            => await _itemsRepository.RemoveByIdAsync(id);
    }
}

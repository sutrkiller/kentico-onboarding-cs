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
        private readonly IIdentifierService _identifierService;

        public ItemsService(IItemsRepository itemsRepository, IIdentifierService identifierService)
        {
            _itemsRepository = itemsRepository;
            _identifierService = identifierService;
        }
        public async Task<Item> AddItemAsync(Item item)
        {
            var newItem = new Item
            {
                Id = await _identifierService.GenerateIdAsync(), 
                Text = item.Text,
            };

            await _itemsRepository.AddAsync(newItem);
            return newItem;
        }
    }
}

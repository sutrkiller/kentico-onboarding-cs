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

        public ItemsService(IItemsRepository itemsRepository, IIdGeneratorService idGeneratorService)
        {
            _itemsRepository = itemsRepository;
            _idGeneratorService = idGeneratorService;
        }
        public async Task<Item> AddItemAsync(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (string.IsNullOrWhiteSpace(text)) throw new ArgumentException("Text cannot be empty", nameof(text));

            var newItem = new Item
            {
                Id = await _idGeneratorService.GenerateIdAsync(), 
                Text = text,

            };

            await _itemsRepository.AddAsync(newItem);
            return newItem;
        }
    }
}

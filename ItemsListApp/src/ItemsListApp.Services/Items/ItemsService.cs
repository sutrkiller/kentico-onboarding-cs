using System;
using System.Threading.Tasks;
using ItemsListApp.Contracts.Models;
using ItemsListApp.Contracts.Repository;

namespace ItemsListApp.Services.Items
{
    public class ItemsService
    {
        private readonly IItemsRepository _itemsRepository;

        public ItemsService(IItemsRepository itemsRepository)
        {
            _itemsRepository = itemsRepository;
        }
        public async Task<Item> AddItemAsync(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (string.IsNullOrWhiteSpace(text)) throw new ArgumentException("Text cannot be empty", nameof(text));

            var newItem = new Item
            {
                Id = Guid.NewGuid(),
                Text = text,

            };

            await _itemsRepository.AddAsync(newItem);
            return newItem;
        }
    }
}

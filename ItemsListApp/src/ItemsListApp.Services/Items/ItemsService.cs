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
        public async Task<Item> AddItemAsync(Item item)
        {
            var newItem = new Item
            {
                Id = await _idGeneratorService.GenerateIdAsync(), 
                Text = item.Text,
            };

            await _itemsRepository.AddAsync(newItem);
            return newItem;
        }
    }
}

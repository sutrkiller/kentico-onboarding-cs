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
        public Task<Item> AddItemAsync(string text)
        {
            throw new NotImplementedException();
        }
    }
}

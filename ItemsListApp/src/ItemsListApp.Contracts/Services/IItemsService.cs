using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ItemsListApp.Contracts.Models;

namespace ItemsListApp.Contracts.Services
{
    public interface IItemsService
    {
        Task<Item> AddItemAsync(Item item);

        Task<Item> GetByIdAsync(Guid id);

        Task<IEnumerable<Item>> GetAllAsync();

        Task<Item> PutAsync(Item item);

        Task<Item> RemoveByIdAsync(Guid id);
    }
}
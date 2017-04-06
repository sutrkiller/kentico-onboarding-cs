using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ItemsListApp.Contracts.Models;

namespace ItemsListApp.Contracts.Repository
{
    public interface IItemsRepository
    {
        Task AddAsync(Item item);

        Task<Item> GetByIdAsync(Guid id);

        Task<IEnumerable<Item>> GetAllAsync();

        Task UpdateAsync(Item item);

        Task<Item> RemoveByIdAsync(Guid id);
    }
}

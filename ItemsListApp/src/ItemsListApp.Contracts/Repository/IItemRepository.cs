using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemsListApp.Contracts.Models;

namespace ItemsListApp.Contracts.Repository
{
    public interface IItemRepository
    {
        Task AddAsync(Item item);

        Task<Item> GetByIdAsync(Guid id);

        Task<IQueryable<Item>> GetAllAsync();

        Task UpdateAsync(Item item);

        Task<Item> RemoveByIdAsync(Guid id);
    }
}

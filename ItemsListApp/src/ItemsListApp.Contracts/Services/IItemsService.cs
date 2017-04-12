using System;
using System.Threading.Tasks;
using ItemsListApp.Contracts.Models;

namespace ItemsListApp.Contracts.Services
{
    public interface IItemsService
    {
        Task<Item> CreateNewAsync(Item item);

        Task<Item> ReplaceExistingAsync(Item item);

        Task<bool> ExistsAsync(Guid id);
    }
}
using System;
using System.Threading.Tasks;
using ItemsListApp.Contracts.Models;

namespace ItemsListApp.Contracts.Services
{
    public interface IExistingItemsService
    {
        Task<Item> ReplaceAsync(Item item);

        Task<bool> ExistsAsync(Guid id);
    }
}
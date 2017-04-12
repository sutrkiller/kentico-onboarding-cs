using System;
using System.Threading.Tasks;
using ItemsListApp.Contracts.Models;

namespace ItemsListApp.Contracts.Services
{
    public interface ICreateItemsService
    {
        Task<Item> CreateNewAsync(Item item);
        Task<Item> CreateNewAsync(Item item, Guid id);
    }
}
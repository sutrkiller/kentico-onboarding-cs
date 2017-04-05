using System.Threading.Tasks;
using ItemsListApp.Contracts.Models;

namespace ItemsListApp.Contracts.Services
{
    public interface IItemsService
    {
        Task<Item> AddItemAsync(Item item);
    }
}
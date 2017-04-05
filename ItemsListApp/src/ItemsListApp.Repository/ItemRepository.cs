using System;
using System.Linq;
using System.Threading.Tasks;
using ItemsListApp.Contracts.Models;
using ItemsListApp.Contracts.Repository;

namespace ItemsListApp.Repository
{
    internal class ItemRepository : IItemRepository
    {
        private static readonly Item[] Items = 
        {
            new Item {Id = new Guid("A3672C82-AF6C-44AD-836E-D1C26A0A6359"), Text = "Dummy text 1"},
            new Item {Id = new Guid("F5CFB0AF-EB26-478B-AF41-7DA314458706"), Text = "Dummy text 2"},
            new Item {Id = new Guid("A77EE2AF-B6A2-456B-8683-A34B37B6E70F"), Text = "Dummy text 3"},
            new Item {Id = new Guid("52AD8CCD-E9E9-420B-8C27-47E695993472"), Text = "Dummy text 4"},
            new Item {Id = new Guid("52637173-064F-4E0C-9CAF-7A43F8AA2D29"), Text = "Dummy text 5"},
        };

        public async Task AddAsync(Item item)
        {
            await Task.CompletedTask;
        }

        public async Task<Item> GetByIdAsync(Guid id)
        {
            return await Task.FromResult(Items[0]);
        }

        public async Task<IQueryable<Item>> GetAllAsync()
        {
            return await Task.FromResult(Items.AsQueryable());
        }

        public async Task UpdateAsync(Item item)
        {
            await Task.CompletedTask;
        }

        public async Task<Item> RemoveByIdAsync(Guid id)
        {
            return await Task.FromResult(Items[3]);
        }
    }
}

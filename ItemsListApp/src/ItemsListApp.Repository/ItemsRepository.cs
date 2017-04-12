using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ItemsListApp.Contracts.Models;
using ItemsListApp.Contracts.Repository;
using MongoDB.Driver;

namespace ItemsListApp.Repository
{
    internal class ItemsRepository : IItemsRepository
    {
        private readonly IMongoCollection<Item> _dbCollection;

        public ItemsRepository(ConnectionOptions connectionOptions)
        {
            const string collectionName = "Items";
            var client = new MongoClient(connectionOptions.ConnectionString);
            var databaseName = MongoUrl.Create(connectionOptions.ConnectionString).DatabaseName;
            var database = client.GetDatabase(databaseName);
            _dbCollection = database.GetCollection<Item>(collectionName);
        }

        public async Task AddAsync(Item item)
            => await _dbCollection.InsertOneAsync(item);

        public async Task<Item> GetByIdAsync(Guid id)
            => await _dbCollection.Find(value => value.Id == id).FirstOrDefaultAsync();

        public async Task<IEnumerable<Item>> GetAllAsync()
            => (await _dbCollection.FindAsync(FilterDefinition<Item>.Empty))
                .ToEnumerable();

        public async Task UpdateAsync(Item item)
            => await _dbCollection.ReplaceOneAsync(value => value.Id == item.Id, item);

        public async Task<Item> RemoveByIdAsync(Guid id)
            => await _dbCollection.FindOneAndDeleteAsync(item => item.Id == id);
    }
}
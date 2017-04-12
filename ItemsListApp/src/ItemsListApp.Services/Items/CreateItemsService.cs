using System;
using System.Threading.Tasks;
using ItemsListApp.Contracts.Models;
using ItemsListApp.Contracts.Repository;
using ItemsListApp.Contracts.Services;

namespace ItemsListApp.Services.Items
{
    internal class CreateItemsService : ICreateItemsService
    {
        private readonly IItemsRepository _itemsRepository;
        private readonly IDateTimeService _dateTimeService;
        private readonly IIdentifierService _identifierService;

        public CreateItemsService(IItemsRepository itemsRepository, IIdentifierService identifierService,
            IDateTimeService dateTimeService)
        {
            _itemsRepository = itemsRepository;
            _identifierService = identifierService;
            _dateTimeService = dateTimeService;
        }

        public async Task<Item> CreateNewAsync(Item item)
            => await CreateNewAsync(item, await _identifierService.GenerateIdAsync());

        public async Task<Item> CreateNewAsync(Item item, Guid id)
        {
            var creationTime = await _dateTimeService.GetCurrentDateAsync();
            var newItem = new Item
            {
                Id = id,
                Text = item.Text,
                CreationTime = creationTime,
                LastUpdateTime = creationTime,
            };

            await _itemsRepository.AddAsync(newItem);
            return newItem;
        }
    }
}

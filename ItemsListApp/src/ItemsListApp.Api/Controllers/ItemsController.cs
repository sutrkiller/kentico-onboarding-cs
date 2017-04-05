using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using ItemsListApp.Contracts.Api;
using ItemsListApp.Contracts.Models;
using ItemsListApp.Contracts.Repository;
using ItemsListApp.Contracts.Services;

namespace ItemsListApp.Api.Controllers
{
    //api/v1/items
    public class ItemsController : ApiController
    {
        private readonly IItemsRepository _itemsesRepository;
        private readonly IItemsService _itemsService;
        private readonly IItemLocationHelper _itemLocationHelper;

        public ItemsController(IItemsRepository itemsesRepository, IItemsService itemsService, IItemLocationHelper itemLocationHelper)
        {
            _itemsesRepository = itemsesRepository;
            _itemsService = itemsService;
            _itemLocationHelper = itemLocationHelper;
        }

        // GET api/items
        public async Task<IHttpActionResult> GetAsync()
        {
            var allItems = await _itemsesRepository.GetAllAsync();

            return Ok(allItems);
        }

        // GET api/v1/items/5
        public async Task<IHttpActionResult> GetAsync(Guid id)
        {
            var item = await _itemsesRepository.GetByIdAsync(id);

            return Ok(item);
        }

        // POST api/v1/items
        public async Task<IHttpActionResult> PostAsync([FromBody] string text)
        {
            var newItem = await _itemsService.AddItemAsync(text);
            var location = _itemLocationHelper.CreateLocation(newItem.Id);

            return Created(location, newItem);
        }

        // PUT api/v1/items/5
        public async Task<IHttpActionResult> PutAsync([FromBody] Item item)
        {
            await _itemsesRepository.UpdateAsync(item);

            return Ok(item);
        }

        // DELETE api/v1/items/5
        public async Task<IHttpActionResult> DeleteAsync(Guid id)
        {
            await _itemsesRepository.RemoveByIdAsync(id);

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
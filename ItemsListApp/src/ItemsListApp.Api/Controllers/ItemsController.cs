using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using ItemsListApp.Contracts.Models;
using ItemsListApp.Contracts.Repository;

namespace ItemsListApp.Api.Controllers
{

    //api/v1/items
    public class ItemsController : ApiController
    {
        private readonly IItemRepository _itemsRepository;

        public ItemsController(IItemRepository itemsRepository)
        {
            _itemsRepository = itemsRepository;
        }

        // GET api/items
        public async Task<IHttpActionResult> GetAsync()
        {
            var allItems = await _itemsRepository.GetAllAsync();

            return Ok(allItems);
        }

        // GET api/v1/items/5
        public async Task<IHttpActionResult> GetAsync(Guid id)
        {
            var item = await _itemsRepository.GetByIdAsync(id);

            return Ok(item);
        }

        // POST api/v1/items
        public async Task<IHttpActionResult> PostAsync([FromBody]string text)
        {
            var newItem = new Item
            {
                Id = new Guid("97DDD880-D922-4A0D-BB07-E35339F4F5BE"),
                Text = text
            };

            await _itemsRepository.AddAsync(new Item());

            var location = new Uri(Request.RequestUri, newItem.Id.ToString());
            return Created( location, newItem);
        }

        // PUT api/v1/items/5
        public async Task<IHttpActionResult> PutAsync([FromBody] Item item)
        {
             await _itemsRepository.UpdateAsync(item);

            return Ok();
        }

        // DELETE api/v1/items/5
        public async Task<IHttpActionResult> DeleteAsync(Guid id)
        {
            await _itemsRepository.RemoveByIdAsync(id);

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
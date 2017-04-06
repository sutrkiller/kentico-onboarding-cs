using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using ItemsListApp.Contracts.Api;
using ItemsListApp.Contracts.Models;
using ItemsListApp.Contracts.Services;

namespace ItemsListApp.Api.Controllers
{
    //api/v1/items
    public class ItemsController : ApiController
    {
        private readonly IItemsService _itemsService;
        private readonly IItemLocationHelper _itemLocationHelper;

        public ItemsController(IItemsService itemsService, IItemLocationHelper itemLocationHelper)
        {
            _itemsService = itemsService;
            _itemLocationHelper = itemLocationHelper;
        }

        // GET api/items
        public async Task<IHttpActionResult> GetAsync()
        {
            //var allItems = await _itemsesRepository.GetAllAsync();
            var allItems = await _itemsService.GetAllAsync();

            return Ok(allItems);
        }

        // GET api/v1/items/5
        public async Task<IHttpActionResult> GetAsync(Guid id)
        {
            ValidateGetId(id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = await _itemsService.GetByIdAsync(id);

            if (item == null)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }

            return Ok(item);
        }

        // POST api/v1/items
        public async Task<IHttpActionResult> PostAsync([FromBody] Item item)
        {
            ValidatePostedItem(item);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newItem = await _itemsService.AddItemAsync(item);
            var location = _itemLocationHelper.CreateLocation(newItem.Id);

            return Created(location, newItem);
        }

        // PUT api/v1/items/5
        public async Task<IHttpActionResult> PutAsync([FromBody] Item item)
        {
            ValidatePutItem(item);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var editedItem = await _itemsService.PutAsync(item);
            if (editedItem == null)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }

            return Ok(editedItem);
        }

        // DELETE api/v1/items/5
        public async Task<IHttpActionResult> DeleteAsync(Guid id)
        {
            await _itemsService.RemoveByIdAsync(id);

            return StatusCode(HttpStatusCode.NoContent);
        }

        private void ValidatePostedItem(Item item)
        {
            if (item == null)
            {
                ModelState.AddModelError(nameof(item), "Body format is not correct");
                return;
            }
            if (item.Id != default(Guid))
            {
                ModelState.AddModelError(nameof(item.Id), "Item must not contain identifier");
            }
            if (string.IsNullOrWhiteSpace(item.Text))
            {
                ModelState.AddModelError(nameof(item.Text), "Item text is not valid");
            }
        }

        private void ValidateGetId(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return;
            }
            if (id == default(Guid))
            {
                ModelState.AddModelError(nameof(id), "Id is not valid");
            }
        }

        private void ValidatePutItem(Item item)
        {
            if (item == null)
            {
                ModelState.AddModelError(nameof(item), "Body format is not correct");
                return;
            }
            if (item.Id == default(Guid))
            {
                ModelState.AddModelError(nameof(item.Id), "Item must contain identifier");
            }
            if (string.IsNullOrWhiteSpace(item.Text))
            {
                ModelState.AddModelError(nameof(item.Text), "Item text is not valid");
            }
        }
    }
}
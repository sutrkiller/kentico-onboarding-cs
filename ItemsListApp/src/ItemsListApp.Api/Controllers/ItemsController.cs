using System;
using System.Net;
using System.Threading;
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
        private readonly ICreateItemsService _createItemsService;
        private readonly IExistingItemsService _existingItemsService;
        private readonly IItemsRepository _itemsRepository;
        private readonly IItemLocationHelper _itemLocationHelper;

        public ItemsController(ICreateItemsService createItemsService, IExistingItemsService existingItemsService, IItemsRepository itemsRepository,
            IItemLocationHelper itemLocationHelper)
        {
            _createItemsService = createItemsService;
            _existingItemsService = existingItemsService;
            _itemsRepository = itemsRepository;
            _itemLocationHelper = itemLocationHelper;
        }

        // GET api/items
        public async Task<IHttpActionResult> GetAsync()
        {
            var allItems = await _itemsRepository.GetAllAsync();
            Thread.Sleep(2000);
            return Ok(allItems);
        }

        // GET api/v1/items/5
        public async Task<IHttpActionResult> GetAsync(Guid id)
        {
            ValidateIdParam(id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = await _itemsRepository.GetByIdAsync(id);

            if (item == null)
            {
                return NotFound();
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

            var newItem = await _createItemsService.CreateNewAsync(item);
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

            if (await _existingItemsService.DoesExistAsync(item.Id))
            {
                var editedItem = await _existingItemsService.ReplaceAsync(item);
                return Ok(editedItem);
            }

            var created = await _createItemsService.CreateNewAsync(item, item.Id);
            var location = _itemLocationHelper.CreateLocation(created.Id);
            return Created(location, created);
        }

        // DELETE api/v1/items/5
        public async Task<IHttpActionResult> DeleteAsync(Guid id)
        {
            ValidateIdParam(id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!await _existingItemsService.DoesExistAsync(id))
            {
                return NotFound();
            }

            await _itemsRepository.RemoveByIdAsync(id);

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
            ValidateNonIdentifierProperties(item);
        }

        private void ValidateIdParam(Guid id)
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
            ValidateNonIdentifierProperties(item);
        }

        private void ValidateNonIdentifierProperties(Item item)
        {
            if (string.IsNullOrWhiteSpace(item.Text))
            {
                ModelState.AddModelError(nameof(item.Text), "Item text is not valid");
            }
            if (item.CreationTime != default(DateTime))
            {
                ModelState.AddModelError(nameof(Item.CreationTime), "Creation time cannot be set through API");
            }
            if (item.LastUpdateTime != default(DateTime))
            {
                ModelState.AddModelError(nameof(Item.LastUpdateTime), "Update time cannot be set through API");
            }
        }
    }
}
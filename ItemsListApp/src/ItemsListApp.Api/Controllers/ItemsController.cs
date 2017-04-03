using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using ItemsListApp.Api.Models;

namespace ItemsListApp.Api.Controllers
{

    //api/v1/items
    public class ItemsController : ApiController
    {
        // GET api/items
        public async Task<IHttpActionResult> GetAsync()
        {
            var model = await Task.FromResult(GetItems());

            return Ok(model);
        }

        // GET api/v1/items/5
        public async Task<IHttpActionResult> GetAsync(Guid id)
        {
            var model = await Task.FromResult(new Item
            {
                Id = id,
                Text = "Text of required item",
            });

            return Ok(model);
        }

        // POST api/v1/items
        public async Task<IHttpActionResult> PostAsync([FromBody]string text)
        {
            var model = await Task.FromResult(new Item
            {
                Id = new Guid("97DDD880-D922-4A0D-BB07-E35339F4F5BE"),
                Text = text
            });

            var location = new Uri(Request.RequestUri, model.Id.ToString());
            return Created( location, model);
        }

        // PUT api/v1/items/5
        public async Task<IHttpActionResult> PutAsync([FromBody] Item item)
        {
            var model = await Task.FromResult(item);

            return Ok(model);
        }

        // DELETE api/v1/items/5
        public async Task<IHttpActionResult> DeleteAsync(Guid id)
        {
            await Task.FromResult(id);

            return StatusCode(HttpStatusCode.NoContent);
        }

        private static IEnumerable<Item> GetItems()
        {
            yield return new Item { Id = new Guid("A3672C82-AF6C-44AD-836E-D1C26A0A6359"), Text = "Dummy text 1" };
            yield return new Item { Id = new Guid("F5CFB0AF-EB26-478B-AF41-7DA314458706"), Text = "Dummy text 2" };
            yield return new Item { Id = new Guid("A77EE2AF-B6A2-456B-8683-A34B37B6E70F"), Text = "Dummy text 3" };
        }
    }
}
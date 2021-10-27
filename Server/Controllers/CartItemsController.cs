using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.ViewModels.Cart;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Route("v1")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        [HttpGet]
        [Route(template: "CartItems")]
        public async Task<IActionResult> GetAsync([FromServices] AppDbContext context)
        {
            return Ok();
        }
        
        [HttpGet("CartItems/{id}")]
        public async Task<IActionResult> GetByIdAsync([FromServices] AppDbContext context, [FromRoute] int id)
        {
            return Ok();
        }

        [HttpPost(template: "CartItems")]
        public async Task<IActionResult> AddItemToCartAsync([FromServices] AppDbContext context, [FromBody] AddToCartViewModel model)
        {
            return BadRequest();
        }

        [HttpPut(template: "CartItems/{id}")]
        public async Task<IActionResult> PutAsync([FromServices] AppDbContext context, [FromBody] UpdateCartViewModel model, [FromRoute] int id)
        {
            return Ok();
        }

        [HttpDelete(template: "CartItems/{id}")]
        public async Task<IActionResult> DeleteAsync([FromServices] AppDbContext context, [FromRoute] int id)
        {
            return Ok();
        }
    }
}

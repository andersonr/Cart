using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;
using Server.ViewModels.Carrinho;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Route("v1")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        [HttpGet]
        [Route(template: "CartItems")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetAsync([FromServices] AppDbContext context)
        {
            var CartItems = await context.CarrinhoItems.AsNoTrackingWithIdentityResolution().Include(nn => nn.Produto).Include(nn => nn.Carrinho).ToListAsync();

            return Ok(CartItems);
        }

        [HttpGet("CartItems/{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetByIdAsync([FromServices] AppDbContext context, [FromRoute] int id)
        {
            var CartItem = await context.CarrinhoItems.AsNoTrackingWithIdentityResolution().Include(nn => nn.Produto).Include(nn => nn.Carrinho).FirstOrDefaultAsync(item => item.Id == id);
            if (CartItem == null)
                return NotFound("Item não encontrado");

            return Ok(CartItem);
        }

        [HttpPost(template: "CartItems")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Post))]
        public async Task<IActionResult> AddItemToCartAsync([FromServices] AppDbContext context, [FromBody] NovoItemCarrinhoViewModel model)
        {
            if (!ModelState.IsValid || !model.IsValidData())
                return BadRequest(model.Notifications);

            var productBase = await context.Produtos.FindAsync(model.Produto.Id);
            if (productBase == null)
                return BadRequest("Produto não encontrado!");

            if (!productBase.IsDisponivel)
                return BadRequest("Produto indisponível!");

            var cartBase = await context.Carrinhos.Include(item => item.CarrinhoItems).FirstOrDefaultAsync(cart => cart.Id == model.Carrinho.Id);
            if (cartBase == null)
                return BadRequest("Carrinho não encontrado!");

            if (!cartBase.Ativo)
                return BadRequest("Carrinho não está mais disponível para adição de itens!");

            if (cartBase.CarrinhoItems.Count(item => item?.Produto?.Id == model.Produto.Id) > 0)
                return BadRequest("Não é possível inserir o mesmo produto mais de uma vez no mesmo carrinho!");

            var productInventory = await context.Estoques.FirstOrDefaultAsync(item => item.Produto == productBase && item.Quantidade > 0);
            if (productInventory == null)
                return BadRequest("Produto sem estoque!");

            var item = new CarrinhoItem
            {
                Carrinho = cartBase,
                Quantidade = model.Quantidade,
                Produto = productBase,
            };
            item.UpdatePrices();

            try
            {
                await context.CarrinhoItems.AddAsync(item);
                cartBase.UpdatePrices();
                await context.SaveChangesAsync();

                return Created($"v1/CartItems/{item.Id}", item);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut(template: "CartItems/{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Put))]
        public async Task<IActionResult> PutAsync([FromServices] AppDbContext context, [FromBody] AtualizaCarrinhoItem model, [FromRoute] int id)
        {
            if (!ModelState.IsValid || !model.IsEntradasValidas())
                return BadRequest(model.Notifications);

            var CartItem = await context.CarrinhoItems.Include(nn => nn.Carrinho).Include(nn => nn.Produto).FirstOrDefaultAsync(item => item.Id == id);
            if (CartItem == null)
                return NotFound("Item não encontrado");

            if (model.Quantidade > 0)
            {
                CartItem.Quantidade = model.Quantidade;
                CartItem.UpdatePrices();
                CartItem.Carrinho.UpdatePrices();

                try
                {
                    context.SaveChanges();
                    return Ok(CartItem);
                }
                catch (System.Exception er)
                {
                    return BadRequest(er.Message);
                }
            }
            else
                return await DeleteAsync(context, id);
        }

        [HttpDelete(template: "CartItems/{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Delete))]
        public async Task<IActionResult> DeleteAsync([FromServices] AppDbContext context, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var item = await context.CarrinhoItems.Include(nn => nn.Carrinho).FirstOrDefaultAsync(nn => nn.Id == id);
            if (item == null)
                return NotFound("Item não encontrado!");

            var cart = item.Carrinho;

            try
            {
                context.CarrinhoItems.Remove(item);
                if (cart.CarrinhoItems.Count > 0)
                    cart.UpdatePrices();
               
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }
    }
}

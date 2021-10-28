using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;
using Server.ViewModels;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [ApiController]
    [Route(template: "v1")]
    public class InventoryController : ControllerBase
    {
        [HttpGet]
        [Route(template: "Inventories")]
        public async Task<IActionResult> GetAsync([FromServices] AppDbContext context)
        {
            var inventories = await context.Estoques.AsNoTracking().Include(ss => ss.Produto).ToListAsync();

            return Ok(inventories);
        }

        [HttpGet]
        [Route(template: "Inventories/{id}")]
        public async Task<IActionResult> GetByIdAsync([FromServices] AppDbContext context, [FromRoute] int id)
        {
            var inventory = await context.Estoques.AsNoTracking().Include(ss => ss.Produto).FirstOrDefaultAsync(item => item.Id == id);

            return inventory == null ? NotFound() : Ok(inventory);
        }

        [HttpPost(template: "Inventories")]
        public async Task<IActionResult> PostAsync([FromServices] AppDbContext context, [FromBody] NovoEstoqueViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var inventoryItem = model.MapTo();
            if (!model.IsValid)
                return BadRequest(model.Notifications);

            var productBase = await context.Produtos.FirstOrDefaultAsync(item => item.Id == inventoryItem.Produto.Id);

            try
            {
                if (productBase == null)
                    await context.Produtos.AddAsync(productBase);
                else
                    context.Produtos.Update(productBase);

                inventoryItem.Produto = productBase;
                await context.Estoques.AddAsync(inventoryItem);
                await context.SaveChangesAsync();

                return Created($"v1/Inventories/{inventoryItem}", inventoryItem);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut(template: "Inventories/{id}")]
        public async Task<IActionResult> PutAsync([FromServices] AppDbContext context, [FromBody] NovoEstoqueViewModel model, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var inventoryItem = await context.Estoques.FirstOrDefaultAsync(item => item.Id == id);

            if (inventoryItem == null)
                return NotFound();

            try
            {
                var item = model.MapTo();
                if (!model.IsValid)
                    return BadRequest(model.Notifications);

                inventoryItem.Quantidade = item.Quantidade;

                var productBase = await context.Produtos.FindAsync(item.Produto.Id);
                if (productBase == null)
                    return NotFound();

                productBase.IsDisponivel = item.Produto.IsDisponivel;
                productBase.PrecoUnitario = item.Produto.PrecoUnitario;
                productBase.Nome = item.Produto.Nome;
                inventoryItem.Produto = productBase;

                context.Produtos.Update(productBase);

                context.Estoques.Update(inventoryItem);
                await context.SaveChangesAsync();

                return Ok(inventoryItem);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete(template: "Inventories/{id}")]
        public async Task<IActionResult> DeleteAsync([FromServices] AppDbContext context, [FromRoute] int id)
        {
            var inventoryItem = await context.Estoques.FindAsync(id);
            if (inventoryItem == null)
                return NotFound();

            try
            {
                context.Estoques.Remove(inventoryItem);
                await context.SaveChangesAsync();

                return Ok();
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }

        //Usar apenas em dev, se necessário
        [HttpPost(template: "CreateInventoryItems")]
        public async Task<IActionResult> CreateInventoryItemsAsync([FromServices] AppDbContext context)
        {
            await context.Produtos.ForEachAsync(async produto =>
            {
                var item = await context.Estoques.AddAsync(new Estoque { Produto = produto, Quantidade = 10 });

                await context.SaveChangesAsync();
            });

            return Ok();
        }
    }
}

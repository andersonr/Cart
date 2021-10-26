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
    public class EstoqueController : ControllerBase
    {
        [HttpGet]
        [Route(template: "Estoques")]
        public async Task<IActionResult> GetAsync([FromServices] AppDbContext context)
        {
            //AsNoTracking é parecido com o With Nolock do SQL, não fica monitorando as alterações durante a resposta, faz a leitura e boa
            var estoques = await context.Estoques.AsNoTracking().ToListAsync();

            return Ok(estoques);
        }

        [HttpGet]
        [Route(template: "Estoques/{id}")]
        public async Task<IActionResult> GetByIdAsync([FromServices] AppDbContext context, [FromRoute] int id)
        {
            //AsNoTracking é parecido com o With Nolock do SQL, não fica monitorando as alterações durante a resposta, faz a leitura e boa
            var estoque = await context.Estoques.AsNoTracking().FirstOrDefaultAsync(item => item.Id == id);

            return estoque == null ? NotFound() : Ok(estoque);
        }

        [HttpPost(template: "Estoques")]
        public async Task<IActionResult> PostAsync([FromServices] AppDbContext context, [FromBody] CreateEstoqueViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var itemEstoque = model.MapTo();
            if (!model.IsValid)
                return BadRequest(model.Notifications);

            var produtoBase = await context.Produtos.FirstOrDefaultAsync(item => item.Id == itemEstoque.Produto.Id);

            try
            {
                if (produtoBase == null)
                    await context.Produtos.AddAsync(produtoBase);
                else
                    context.Produtos.Update(produtoBase);

                itemEstoque.Produto = produtoBase;
                await context.Estoques.AddAsync(itemEstoque);
                await context.SaveChangesAsync();

                return Created($"v1/Estoques/{itemEstoque}", itemEstoque);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);//Correto não é BadRequest, ver um melhor, que mais se adequa
            }
        }

        [HttpPut(template: "Estoques/{id}")]
        public async Task<IActionResult> PutAsync([FromServices] AppDbContext context, [FromBody] CreateEstoqueViewModel model, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var itemEstoque = await context.Estoques.FirstOrDefaultAsync(item => item.Id == id);

            if (itemEstoque == null)
                return NotFound();

            try
            {
                var item = model.MapTo();
                if (!model.IsValid)
                    return BadRequest(model.Notifications);

                itemEstoque.Quantidade = item.Quantidade;

                //var produtoBase = await context.Produtos.FirstOrDefaultAsync(prod => prod.Id == item.Produto.Id);
                var produtoBase = await context.Produtos.FindAsync(item.Produto.Id);//  FirstOrDefaultAsync(prod => prod.Id == item.Produto.Id);
                if (produtoBase == null)
                    return NotFound();

                produtoBase.IsDisponivel = item.Produto.IsDisponivel;
                produtoBase.PrecoUnitario = item.Produto.PrecoUnitario;
                produtoBase.Nome = item.Produto.Nome;
                itemEstoque.Produto = produtoBase;

                context.Produtos.Update(produtoBase);

                //itemEstoque.Produto = produtoBase;
                context.Estoques.Update(itemEstoque);
                await context.SaveChangesAsync();

                return Ok(itemEstoque);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);//Correto não é BadRequest, ver um melhor, que mais se adequa
            }
        }

        //Usar apenas em dev, se necessário
        [HttpDelete(template: "Estoques/{id}")]
        public async Task<IActionResult> DeleteAsync([FromServices] AppDbContext context, [FromRoute] int id)
        {
            //var itemEstoque = await context.Estoques.FirstOrDefaultAsync(item => item.Id == id);
            var itemEstoque = await context.Estoques.ToArrayAsync();
            try
            {
                context.Estoques.RemoveRange(itemEstoque);
                //context.Estoques.Remove(itemEstoque);
                await context.SaveChangesAsync();

                return Ok();
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost(template: "PreencheEstoque")]
        public async Task<IActionResult> PreencheEstoqueAsync([FromServices] AppDbContext context)
        {
            await context.Produtos.ForEachAsync(async produto =>
            {
                var item = await context.Estoques.AddAsync(new Estoque { Produto = produto, Quantidade = 10 });

                await context.SaveChangesAsync();
                // await PostAsync(context, new CreateEstoqueViewModel { Produto = produto, Quantidade = 10 });
            });

            return Ok();
        }
    }
}

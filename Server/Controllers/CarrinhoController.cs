using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.ViewModels;
using Server.ViewModels.Carrinho;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [ApiController]
    [Route(template: "v1")]
    public class CarrinhoController : ControllerBase
    {

        [HttpGet]
        [Route(template: "Carrinhos")]
        public async Task<IActionResult> GetAsync([FromServices] AppDbContext context)
        {
            //AsNoTracking é parecido com o With Nolock do SQL, não fica monitorando as alterações durante a resposta, faz a leitura e boa
            var carrinho = await context.Carrinhos.AsNoTracking().ToListAsync();

            return Ok(carrinho);
        }

        [HttpGet]
        [Route(template: "Carrinhos/{id}")]
        public async Task<IActionResult> GetByIdAsync([FromServices] AppDbContext context, [FromRoute] int id)
        {
            var carrinho = await context.Carrinhos.AsNoTracking().FirstOrDefaultAsync(item => item.Id == id);

            return carrinho == null ? NotFound() : Ok(carrinho);
        }

        [HttpPost(template: "Carrinhos")]
        public async Task<IActionResult> PostAsync([FromServices] AppDbContext context, [FromBody] CreateCarrinhoViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var carrinho = model.MapTo();
            if (!model.IsValid)
                return BadRequest(model.Notifications);

            try
            {
                await context.Carrinhos.AddAsync(carrinho);
                await context.SaveChangesAsync();

                return Created($"v1/Carrinhos/{carrinho.Id}", carrinho);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);//Correto não é BadRequest, ver um melhor, que mais se adequa
            }
        }

        [HttpPut(template: "Carrinhos/{id}")]
        public async Task<IActionResult> PutAsync([FromServices] AppDbContext context, [FromBody] UpdateCarrinhoViewModel model, [FromRoute] int id)
        {
            //if (!ModelState.IsValid)
            //    return BadRequest();

            //var carrinho = await context.Carrinhos.FirstOrDefaultAsync(item => item.Id == id);

            //if (carrinho == null)
            return NotFound();

            //try
            //{
            //    var produtoRecebido = model.MapTo();
            //    if (!model.IsValid)
            //        return BadRequest(model.Notifications);

            //    carrinho.Nome = produtoRecebido.Nome;
            //    carrinho.IsDisponivel = produtoRecebido.IsDisponivel;
            //    carrinho.PrecoUnitario = produtoRecebido.PrecoUnitario;

            //    context.Produtos.Update(carrinho);
            //    await context.SaveChangesAsync();

            //    return Ok(carrinho);
            //}
            //catch (System.Exception e)
            //{
            //    return BadRequest(e.Message);//Correto não é BadRequest, ver um melhor, que mais se adequa
            //}
        }

        //[HttpDelete(template: "Carrinhos/{id}")]
        //public async Task<IActionResult> DeleteAsync([FromServices] AppDbContext context, [FromRoute] int id)
        //{
        //    var produto = await context.Produtos.FirstOrDefaultAsync(item => item.Id == id);

        //    try
        //    {
        //        context.Produtos.Remove(produto);
        //        await context.SaveChangesAsync();

        //        return Ok();
        //    }
        //    catch (System.Exception)
        //    {
        //        return BadRequest();
        //    }
        //}

    }
}

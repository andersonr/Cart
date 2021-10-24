using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.ViewModels;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [ApiController]
    [Route(template: "v1")]
    public class ProdutoController : ControllerBase
    {
        [HttpGet]
        [Route(template: "Produtos")]
        public async Task<IActionResult> GetAsync([FromServices] AppDbContext context)
        {
            //AsNoTracking é parecido com o With Nolock do SQL, não fica monitorando as alterações durante a resposta, faz a leitura e boa
            var produtos = await context.Produtos.AsNoTracking().ToListAsync();

            return Ok(produtos);
        }

        [HttpGet]
        [Route(template: "Produtos/{id}")]
        public async Task<IActionResult> GetByIdAsync([FromServices] AppDbContext context, [FromRoute] int id)
        {
            //AsNoTracking é parecido com o With Nolock do SQL, não fica monitorando as alterações durante a resposta, faz a leitura e boa
            var produtos = await context.Produtos.AsNoTracking().FirstOrDefaultAsync(item => item.Id == id);

            return produtos == null ? NotFound() : Ok(produtos);
        }

        [HttpPost(template: "Produtos")]
        public async Task<IActionResult> PostAsync([FromServices] AppDbContext context, [FromBody] CreateProdutoViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var produto = model.MapTo();
            if (!model.IsValid)
                return BadRequest(model.Notifications);

            try
            {
                await context.Produtos.AddAsync(produto);
                await context.SaveChangesAsync();

                return Created($"v1/Produtos/{produto.Id}", produto);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);//Correto não é BadRequest, ver um melhor, que mais se adequa
            }
        }

        [HttpPut(template: "Produtos/{id}")]
        public async Task<IActionResult> PutAsync([FromServices] AppDbContext context, [FromBody] CreateProdutoViewModel model, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var produto = await context.Produtos.FirstOrDefaultAsync(item => item.Id == id);

            if (produto == null)
                return NotFound();

            try
            {
                var produtoRecebido = model.MapTo();
                if (!model.IsValid)
                    return BadRequest(model.Notifications);

                produto.Nome = produtoRecebido.Nome;
                produto.IsDisponivel = produtoRecebido.IsDisponivel;
                produto.PrecoUnitario = produtoRecebido.PrecoUnitario;
                
                context.Produtos.Update(produto);
                await context.SaveChangesAsync();

                return Ok(produto);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);//Correto não é BadRequest, ver um melhor, que mais se adequa
            }
        }

        //[HttpDelete(template: "Produtos/{id}")]
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

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.ViewModels;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [ApiController]
    [Route(template: "v1")]
    public class UsuarioController : ControllerBase
    {
        [HttpGet]
        [Route(template: "Usuarios")]
        public async Task<IActionResult> GetAsync([FromServices] AppDbContext context)
        {
            var usuarios = await context.Usuarios.AsNoTracking().ToListAsync();

            return Ok(usuarios);
        }

        [HttpGet]
        [Route(template: "Usuarios/{id}")]
        public async Task<IActionResult> GetByIdAsync([FromServices] AppDbContext context, [FromRoute] int id)
        {
            var usuario = await context.Usuarios.AsNoTracking().FirstOrDefaultAsync(item => item.Id == id);

            return usuario == null ? NotFound() : Ok(usuario);
        }

        [HttpPost(template: "Usuarios")]
        public async Task<IActionResult> PostAsync([FromServices] AppDbContext context, [FromBody] CreateUsuarioViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var usuario = model.MapTo();
            if (!model.IsValid)
                return BadRequest(model.Notifications);

            try
            {
                await context.Usuarios.AddAsync(usuario);
                await context.SaveChangesAsync();

                return Created($"v1/Usuarios/{usuario.Id}", usuario);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);//Correto não é BadRequest, ver um melhor, que mais se adequa
            }
        }

        [HttpPut(template: "Usuarios/{id}")]
        public async Task<IActionResult> PutAsync([FromServices] AppDbContext context, [FromBody] CreateUsuarioViewModel model, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var usuario = await context.Usuarios.FirstOrDefaultAsync(item => item.Id == id);

            if (usuario == null)
                return NotFound();

            try
            {
                var usuarioRecebido = model.MapTo();
                if (!model.IsValid)
                    return BadRequest(model.Notifications);

                usuario.Nome = usuarioRecebido.Nome;

                context.Usuarios.Update(usuario);
                await context.SaveChangesAsync();

                return Ok(usuario);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);//Correto não é BadRequest, ver um melhor, que mais se adequa
            }
        }

        //[HttpDelete(template: "Usuarios/{id}")]
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

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.ViewModels;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [ApiController]
    [Route(template: "v1")]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        [Route(template: "Users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAsync([FromServices] AppDbContext context)
        {
            try
            {
                var users = await context.Usuarios.AsNoTracking().ToListAsync();

                return Ok(users);
            }
            catch (System.Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpGet]
        [Route(template: "Users/{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetByIdAsync([FromServices] AppDbContext context, [FromRoute] int id)
        {
            var user = await context.Usuarios.AsNoTracking().FirstOrDefaultAsync(item => item.Id == id);

            return user == null ? NotFound() : Ok(user);
        }

        [HttpPost(template: "Users")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Post))]
        public async Task<IActionResult> PostAsync([FromServices] AppDbContext context, [FromBody] NovoUsuarioViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = model.MapTo();
            if (!model.IsValid)
                return BadRequest(model.Notifications);

            try
            {
                await context.Usuarios.AddAsync(user);
                await context.SaveChangesAsync();

                return Created($"v1/Users/{user.Id}", user);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut(template: "Users/{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Put))]
        public async Task<IActionResult> PutAsync([FromServices] AppDbContext context, [FromBody] NovoUsuarioViewModel model, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await context.Usuarios.FirstOrDefaultAsync(item => item.Id == id);

            if (user == null)
                return NotFound();

            try
            {
                var userModel = model.MapTo();
                if (!model.IsValid)
                    return BadRequest(model.Notifications);

                user.Nome = userModel.Nome;

                context.Usuarios.Update(user);
                await context.SaveChangesAsync();

                return Ok(user);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}

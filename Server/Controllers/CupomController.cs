using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;
using Server.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [ApiController]
    [Route(template: "v1")]
    public class CupomController : ControllerBase
    {
        [HttpGet]
        [Route(template: "Cupons")]
        public async Task<IActionResult> GetAsync([FromServices] AppDbContext context)
        {
            var cupons = await context.Cupons.AsNoTracking().ToListAsync();

            return Ok(cupons);
        }

        [HttpGet]
        [Route(template: "Cupons/{id}")]
        public async Task<IActionResult> GetByIdAsync([FromServices] AppDbContext context, [FromRoute] int id)
        {
            var cupom = await context.Cupons.AsNoTracking().FirstOrDefaultAsync(item => item.Id == id);

            return cupom == null ? NotFound() : Ok(cupom);
        }

        [HttpPost(template: "Cupons")]
        public async Task<IActionResult> PostAsync([FromServices] AppDbContext context, [FromBody] NovoCupomViewModel model)
        {
            if (!ModelState.IsValid && !model.IsValidData())
                return BadRequest(model.Notifications);

            var cupom = new Cupom
            {
                IsAtivo = model.IsAtivo,
                Codigo = model.Codigo,
                PercentualDesconto = model.PercentualDesconto
            };

            try
            {
                await context.Cupons.AddAsync(cupom);
                await context.SaveChangesAsync();

                return Created($"v1/Cupons/{cupom.Id}", cupom);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut(template: "Cupons/{id}")]
        public async Task<IActionResult> PutAsync([FromServices] AppDbContext context, [FromBody] NovoCupomViewModel model, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var cupom = await context.Cupons.FirstOrDefaultAsync(item => item.Id == id);

            if (cupom == null)
                return NotFound();

            try
            {
                cupom.Codigo = model.Codigo;
                cupom.PercentualDesconto = model.PercentualDesconto;
                cupom.IsAtivo = model.IsAtivo;

                context.Carrinhos.ToListAsync().Result.FindAll(cart => cart.Ativo && cart.Cupom == cupom).ForEach(cart => cart.UpdatePrices());
                context.Cupons.Update(cupom);
                await context.SaveChangesAsync();

                return Ok(cupom);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete(template: "Cupons/{id}")]
        public async Task<IActionResult> DeleteAsync([FromServices] AppDbContext context, [FromRoute] int id)
        {
            var cupom = await context.Cupons.FirstOrDefaultAsync(item => item.Id == id);

            try
            {
                context.Carrinhos.ToListAsync().Result.FindAll(cart => cart.Ativo && cart.Cupom == cupom).ForEach(cart => cart.UpdatePrices());
                context.Cupons.Remove(cupom);
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



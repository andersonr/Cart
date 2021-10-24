﻿using Microsoft.AspNetCore.Mvc;
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
            //AsNoTracking é parecido com o With Nolock do SQL, não fica monitorando as alterações durante a resposta, faz a leitura e boa
            var cupons = await context.Cupons.AsNoTracking().ToListAsync();

            return Ok(cupons);
        }

        [HttpGet]
        [Route(template: "Cupons/{id}")]
        public async Task<IActionResult> GetByIdAsync([FromServices] AppDbContext context, [FromRoute] int id)
        {
            //AsNoTracking é parecido com o With Nolock do SQL, não fica monitorando as alterações durante a resposta, faz a leitura e boa
            var cupom = await context.Cupons.AsNoTracking().FirstOrDefaultAsync(item => item.Id == id);

            return cupom == null ? NotFound() : Ok(cupom);
        }

        [HttpPost(template: "Cupons")]
        public async Task<IActionResult> PostAsync([FromServices] AppDbContext context, [FromBody] CreateCupomViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var cupom = new Cupom
            {
                IsAtivo = false,
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
                return BadRequest(e.Message);//Correto não é BadRequest, ver um melhor, que mais se adequa
            }
        }

        [HttpPut(template: "Cupons/{id}")]
        public async Task<IActionResult> PutAsync([FromServices] AppDbContext context, [FromBody] CreateCupomViewModel model, [FromRoute] int id)
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

                context.Cupons.Update(cupom);
                await context.SaveChangesAsync();

                return Ok(cupom);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);//Correto não é BadRequest, ver um melhor, que mais se adequa
            }
        }

        [HttpDelete(template: "cupons/{id}")]
        public async Task<IActionResult> PutAsync([FromServices] AppDbContext context, [FromRoute] int id)
        {
            var cupom = await context.Cupons.FirstOrDefaultAsync(item => item.Id == id);

            try
            {
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


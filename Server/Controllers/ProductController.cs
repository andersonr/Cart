using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.ViewModels;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [ApiController]
    [Route(template: "v1")]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        [Route(template: "Products")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetAsync([FromServices] AppDbContext context)
        {
            var products = await context.Produtos.AsNoTracking().ToListAsync();

            return Ok(products);
        }

        [HttpGet]
        [Route(template: "Products/{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetByIdAsync([FromServices] AppDbContext context, [FromRoute] int id)
        {
            var products = await context.Produtos.AsNoTracking().FirstOrDefaultAsync(item => item.Id == id);

            return products == null ? NotFound() : Ok(products);
        }

        [HttpPost(template: "Products")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Post))]
        public async Task<IActionResult> PostAsync([FromServices] AppDbContext context, [FromBody] NovoProdutoViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var product = model.MapTo();
            if (!model.IsValid)
                return BadRequest(model.Notifications);

            try
            {
                await context.Produtos.AddAsync(product);
                await context.SaveChangesAsync();

                return Created($"v1/Products/{product.Id}", product);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut(template: "Products/{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Put))]
        public async Task<IActionResult> PutAsync([FromServices] AppDbContext context, [FromBody] NovoProdutoViewModel model, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var product = await context.Produtos.FirstOrDefaultAsync(item => item.Id == id);

            if (product == null)
                return NotFound();

            try
            {
                var productModel = model.MapTo();
                if (!model.IsValid)
                    return BadRequest(model.Notifications);

                product.Nome = productModel.Nome;
                product.IsDisponivel = productModel.IsDisponivel;
                product.PrecoUnitario = productModel.PrecoUnitario;

                context.Produtos.Update(product);
                await context.SaveChangesAsync();

                return Ok(product);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}

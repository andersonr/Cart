using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.ViewModels.Carrinho;
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Controllers
{
    [ApiController]
    [Route(template: "v1")]
    public class CartController : ControllerBase
    {
        private DI.ITrafficLock service;
        public CartController(DI.ITrafficLock service)
        {
            this.service = service;
        }

        [HttpGet]
        [Route(template: "Cart")]
        public async Task<IActionResult> GetAsync([FromServices] AppDbContext context)
        {
            var valorCookie = HttpContext.Request.Cookies["tokenID"]?.ToString();

            if (valorCookie == null && !service.TrafficLock.TryGetValue(HttpContext.Session.Id, out valorCookie))
                return NotFound("Carrinho não encontrado!");

            //var carrinho = await context.Carrinhos.FirstOrDefaultAsync(nn => nn.Token == valorCookie);

            //return Ok(carrinho);

            var carrinho = await context.Carrinhos.ToListAsync();

            return Ok(carrinho);
        }

        [HttpPost(template: "Cart")]
        public async Task<IActionResult> PostAsync([FromServices] AppDbContext context, [FromBody] AddToCartViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (!model.IsEntradasValidas())
                return BadRequest(model.Notifications);

            var tokenCarrinho = string.Empty;

            if (HttpContext.Request.Cookies["tokenID"] == null)
            {
                var isAlreadyCreatedCart = (service.TrafficLock.ContainsKey(HttpContext.Session.Id));

                tokenCarrinho = isAlreadyCreatedCart ? service.TrafficLock[HttpContext.Session.Id] : Guid.NewGuid().ToString();
                if (isAlreadyCreatedCart)
                    service.TrafficLock.TryAdd(HttpContext.Session.Id, tokenCarrinho);

                var dataExpiracaoCookie = DateTime.Now + TimeSpan.FromDays(3650);
                HttpContext.Response.Cookies.Append("tokenID",
                                                    tokenCarrinho,
                                                    new CookieOptions
                                                    {
                                                        Expires = new DateTimeOffset(dataExpiracaoCookie),
                                                        IsEssential = true
                                                    });

                if (!isAlreadyCreatedCart)
                {
                    var novoCarrinho = new CreateCarrinhoViewModel
                    {
                        Ativo = true,
                        Token = tokenCarrinho,
                    };
                    var carrinhoController = new CarrinhoController();
                    var criacaoCarrinho = await carrinhoController.PostAsync(context, novoCarrinho);
                    if (criacaoCarrinho != null && !(criacaoCarrinho is CreatedResult))
                        return criacaoCarrinho;
                }
            }
            else
                tokenCarrinho = HttpContext.Request.Cookies["tokenID"].ToString();

            var cart = await context.Carrinhos.FirstOrDefaultAsync(nn => nn.Token == tokenCarrinho);
            var produto = await context.Produtos.FindAsync(model.ProdutoID);
            if (produto == null)
                return BadRequest("Produto não encontrado!");

            if (!produto.IsDisponivel)
                return BadRequest("Produto não está mais disponível!");

            var estoqueProduto = await context.Estoques.FirstOrDefaultAsync(nn => nn.Produto == produto);
            if (estoqueProduto == null || estoqueProduto.Quantidade <= 0)
                return BadRequest("Produto sem estoque!");

            var item = new CarrinhoItem { Carrinho = cart, Produto = produto, Quantidade = model.Qtdade, PrecoTotalItem = (model.Qtdade * produto.PrecoUnitario) };

            try
            {

                await context.CarrinhoItems.AddAsync(item);
                await context.SaveChangesAsync();

                return Created($"v1/Carts/{cart.Id}", cart);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);//Correto não é BadRequest, ver um melhor, que mais se adequa
            }
        }
    }
}

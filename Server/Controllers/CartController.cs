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
        private const string CookieTokenCart = "tokenID";
        private DI.ITrafficLock service;
        public CartController(DI.ITrafficLock service)
        {
            this.service = service;
        }

        [HttpGet]
        [Route(template: "Cart")]
        public async Task<IActionResult> GetAsync([FromServices] AppDbContext context)
        {
            var valorCookie = HttpContext.Request.Cookies[CookieTokenCart]?.ToString();

            if (valorCookie == null && !service.TrafficLock.TryGetValue(HttpContext.Session.Id, out valorCookie))
                return NotFound("Carrinho não encontrado!");

            var carrinho = await context.Carrinhos.FirstOrDefaultAsync(nn => nn.Token == valorCookie);
            return Ok(carrinho);

            //var carrinho = await context.Carrinhos.ToListAsync();
            //return Ok(carrinho);
        }

        private async Task<IActionResult> CreateCartDefinitionAsync(AppDbContext context, CreateCarrinhoViewModel model)
        {
            var carrinhoController = new CarrinhoController();
            var criacaoCarrinho = await carrinhoController.PostAsync(context, model);

            if (criacaoCarrinho != null && !(criacaoCarrinho is CreatedResult))
                throw new Exception($"Erro ao criar carrinho! {criacaoCarrinho.ToString()}");

            return criacaoCarrinho;
        }

        private async Task<Carrinho> GetCartAsync(AppDbContext context)
        {
            var cartToken = string.Empty;

            if (HttpContext.Request.Cookies[CookieTokenCart] == null)
            {
                //Verifica se existe outra chamada a partir do client fazendo a criação do carrinho e token
                var isAlreadyCreatedCart = (service.TrafficLock.ContainsKey(HttpContext.Session.Id));

                cartToken = isAlreadyCreatedCart ? service.TrafficLock[HttpContext.Session.Id] : Guid.NewGuid().ToString();
                if (!isAlreadyCreatedCart)
                    service.TrafficLock.TryAdd(HttpContext.Session.Id, cartToken);

                CreateCookieCart(cartToken);

                if (!isAlreadyCreatedCart)
                    await CreateCartDefinitionAsync(context, new CreateCarrinhoViewModel { Ativo = true, Token = cartToken, });
            }
            else
                cartToken = HttpContext.Request.Cookies[CookieTokenCart].ToString();

            return await context.Carrinhos.FirstOrDefaultAsync(nn => nn.Token == cartToken);
        }

        private void CreateCookieCart(string tokenCarrinho)
        {
            try
            {
                var expirationCookieDate = DateTime.Now + TimeSpan.FromDays(3650);

                HttpContext.Response.Cookies.Append(CookieTokenCart,
                                                    tokenCarrinho,
                                                    new CookieOptions
                                                    {
                                                        Expires = new DateTimeOffset(expirationCookieDate),
                                                        IsEssential = true
                                                    });
            }
            catch (Exception err)
            {
                throw new Exception("Erro ao gerar token identificador do carrinho!", err);
            }
        }

        [HttpPost(template: "Cart")]
        public async Task<IActionResult> AddToCartAsync([FromServices] AppDbContext context, [FromBody] AddToCartViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (!model.IsEntradasValidas())
                return BadRequest(model.Notifications);

            var cart = await GetCartAsync(context);

            if (cart == null)
            {
                model.AddNotification("Carrinho não encontrado!", "Carrinho não encontrado!");
                return NotFound(model.Notifications);
            }

            var produto = await context.Produtos.FindAsync(model.ProdutoID);
            if (produto == null)
            {
                model.AddNotification("Produto não encontrado!", "Produto não encontrado!");
                return BadRequest(model.Notifications);
            }

            if (!produto.IsDisponivel)
            {
                model.AddNotification("Produto não está mais disponível!", "Produto não está mais disponível!");
                return BadRequest(model.Notifications);
            }

            var estoqueProduto = await context.Estoques.FirstOrDefaultAsync(nn => nn.Produto == produto);
            if (estoqueProduto == null || estoqueProduto.Quantidade <= 0)
            {
                model.AddNotification("Produto sem estoque!", "Produto sem estoque!");

                return BadRequest(model.Notifications);
            }

            var item = new CarrinhoItem {
                Carrinho = cart, 
                Produto = produto, 
                Quantidade = model.Qtdade, 
                PrecoTotalItem = (model.Qtdade * produto.PrecoUnitario) 
            };

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

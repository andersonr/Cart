using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.ViewModels.Cart;
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Server.Models;
using System.Linq;

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
            var carts = await context.Carrinhos.AsNoTracking().ToListAsync();
            return Ok(carts);
        }

        [HttpGet]
        [Route(template: "CurrentCart")]
        public async Task<IActionResult> GetCurrentCartAsync([FromServices] AppDbContext context)
        {
            var cookieIdentifierValue = HttpContext.Request.Cookies[CookieTokenCart]?.ToString();

            if (cookieIdentifierValue == null && !service.TrafficLock.TryGetValue(HttpContext.Session.Id, out cookieIdentifierValue))
                return NotFound("Carrinho não encontrado!");

            var cart = await context.Carrinhos.FirstOrDefaultAsync(nn => nn.Token == cookieIdentifierValue);

            return (cart == null) ? NotFound("Carrinho não encontrado!") : Ok(cart);
        }

        [HttpGet]
        [Route(template: "Cart/{id}")]
        public async Task<IActionResult> GetByIdAsync([FromServices] AppDbContext context, [FromRoute] int id)
        {
            var cart = await context.Carrinhos.AsNoTracking().FirstOrDefaultAsync(item => item.Id == id);

            return cart == null ? NotFound("Carrinho não encontrado!") : Ok(cart);
        }

        [HttpPost(template: "Cart")]
        public async Task<IActionResult> CartAsync([FromServices] AppDbContext context, [FromBody] NewCartViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var newCart = model.MapTo();
            if (!model.IsValid)
                return BadRequest(model.Notifications);

            if (service.TrafficLock.ContainsKey(HttpContext.Session.Id))
                service.TrafficLock[HttpContext.Session.Id] = newCart.Token;

            Task deactivatingOldCarts = null;
            if (HttpContext.Request.Cookies[CookieTokenCart] != null)
            {
                deactivatingOldCarts = DeactiveCartsByCookieToken(context, HttpContext.Request.Cookies[CookieTokenCart].ToString());
                //Será que devo remover do Response e adicionar de novo? Como atualizar?
                HttpContext.Response.Cookies.Delete(CookieTokenCart);
                CreateCookieCart(newCart.Token);
            }
            else
                CreateCookieCart(newCart.Token);

            if (deactivatingOldCarts != null)
                await deactivatingOldCarts;

            try
            {
                await context.Carrinhos.AddAsync(newCart);
                await context.SaveChangesAsync();

                return Created($"v1/Cart/{newCart.Id}", newCart);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);//Ver uma alternativa melhor
            }
        }

        [HttpPost(template: "AddToCart")]
        public async Task<IActionResult> GetCartAsync([FromServices] AppDbContext context, [FromBody] AddToCartViewModel model)
        {
            //if (!ModelState.IsValid)
            //    return BadRequest();

            //if (!model.IsEntradasValidas())
            //    return BadRequest(model.Notifications);

            //var cart = await GetCartAsync(context);

            //if (cart == null)
            //{
            //    model.AddNotification("Carrinho não encontrado!", "Carrinho não encontrado!");
            //    return NotFound(model.Notifications);
            //}

            //var produto = await context.Produtos.FindAsync(model.ProdutoID);
            //if (produto == null)
            //{
            //    model.AddNotification("Produto não encontrado!", "Produto não encontrado!");
            //    return BadRequest(model.Notifications);
            //}

            //if (!produto.IsDisponivel)
            //{
            //    model.AddNotification("Produto não está mais disponível!", "Produto não está mais disponível!");
            //    return BadRequest(model.Notifications);
            //}

            //var estoqueProduto = await context.Estoques.FirstOrDefaultAsync(nn => nn.Produto == produto);
            //if (estoqueProduto == null || estoqueProduto.Quantidade <= 0)
            //{
            //    model.AddNotification("Produto sem estoque!", "Produto sem estoque!");

            //    return BadRequest(model.Notifications);
            //}

            //var item = new CarrinhoItem
            //{
            //    Carrinho = cart,
            //    Produto = produto,
            //    Quantidade = model.Qtdade,
            //    PrecoTotalItem = (model.Qtdade * produto.PrecoUnitario)
            //};

            //try
            //{
            //    await context.CarrinhoItems.AddAsync(item);

            //    var totalAtual = cart.CarrinhoItems.Sum(item => item.PrecoTotalItem);
            //    cart.PrecoTotal = totalAtual;

            //    await context.SaveChangesAsync();

            //    return Created($"v1/Carts/{cart.Id}", cart);
            //}
            //catch (System.Exception e)
            //{
            //    return BadRequest(e.Message);//Correto não é BadRequest, ver um melhor, que mais se adequa
            //}
            return BadRequest();
        }

        [HttpPut(template: "Cart/{id}")]
        public async Task<IActionResult> PutAsync([FromServices] AppDbContext context, [FromBody] UpdateCartViewModel model, [FromRoute] int id)
        {
            if (!ModelState.IsValid || !model.IsValidEntryData())
                return BadRequest(model.Notifications);

            var cart = await context.Carrinhos.FindAsync(id);
            if (cart == null)
                return NotFound("Carrinho não encontrado!");

            if (model.Cupom != null)
            {
                var total = await context.CarrinhoItems.Where(item => item.Carrinho == cart).SumAsync(nn => nn.PrecoTotalItem);
                cart.PrecoTotal = total;
                cart.PrecoTotalDesconto = total > 0 ? (total - ((total * model.Cupom.PercentualDesconto) / 100)) : total;
            }

            cart.Usuario = model.Usuario;
            cart.Ativo = model.Ativo;

            return Ok();
        }

        //private async Task<IActionResult> CreateCartDefinitionAsync(AppDbContext context, NewCartViewModel model)
        //{
        //    var carrinhoController = new CarrinhoController();
        //    var criacaoCarrinho = await carrinhoController.PostAsync(context, model);

        //    if (criacaoCarrinho != null && !(criacaoCarrinho is CreatedResult))
        //        throw new Exception($"Erro ao criar carrinho! {criacaoCarrinho.ToString()}");

        //    return criacaoCarrinho;
        //}

        //private async Task<Carrinho> GetCartAsync(AppDbContext context)
        //{
        //    var cartToken = string.Empty;

        //    if (HttpContext.Request.Cookies[CookieTokenCart] == null)
        //    {
        //        //Verifica se existe outra chamada a partir do client fazendo a criação do carrinho e token
        //        var isAlreadyCreatedCart = (service.TrafficLock.ContainsKey(HttpContext.Session.Id));

        //        cartToken = isAlreadyCreatedCart ? service.TrafficLock[HttpContext.Session.Id] : Guid.NewGuid().ToString();
        //        if (!isAlreadyCreatedCart)
        //            service.TrafficLock.TryAdd(HttpContext.Session.Id, cartToken);

        //        CreateCookieCart(cartToken);

        //        if (!isAlreadyCreatedCart)
        //            await CreateCartDefinitionAsync(context, new NewCartViewModel {  });
        //    }
        //    else
        //        cartToken = HttpContext.Request.Cookies[CookieTokenCart].ToString();

        //    return await context.Carrinhos.Include(itens => itens.CarrinhoItems).FirstOrDefaultAsync(nn => nn.Token == cartToken);
        //}

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
        private async Task DeactiveCartsByCookieToken(AppDbContext context, string oldCookie)
        {
            var activeCarts = context.Carrinhos.Where(nn => nn.Ativo && nn.Token == oldCookie);
            if (activeCarts.Any())
            {
                await activeCarts.ForEachAsync(cart => cart.Ativo = false);
            }
        }
    }
}

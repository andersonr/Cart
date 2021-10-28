using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.ViewModels.Carrinho;
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Server.Models;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

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
            var cart = await context.Carrinhos.AsNoTrackingWithIdentityResolution()
                                                .Include(nn => nn.Cupom)
                                                .Include(nn => nn.Usuario)
                                                .Include(nn => nn.CarrinhoItems).ThenInclude(c => c.Produto)
                                                .FirstOrDefaultAsync(item => item.Id == id);
            if (cart == null)
                return NotFound("Carrinho não encontrado!");

            return Ok(new { cart, Items = cart.CarrinhoItems });
        }

        [HttpGet]
        [Route(template: "CartTotals/{id}")]
        public async Task<IActionResult> GetTotalsByIdAsync([FromServices] AppDbContext context, [FromRoute] int id)
        {
            var cart = await context.Carrinhos.Include(nn => nn.Cupom).Include(nn => nn.CarrinhoItems).FirstOrDefaultAsync(item => item.Id == id);

            if (cart == null)
                return NotFound("Carrinho não encontrado!");

            cart.UpdatePrices();
            try
            {
                context.Carrinhos.Update(cart);
            }
            catch (Exception er)
            {
                return BadRequest(er.Message);
            }

            var items = context.CarrinhoItems.Include(nn => nn.Produto).AsNoTracking().Where(item => item.Carrinho.Id == cart.Id);
            var descountToCalculateSubItem = cart.Cupom != null ? cart.Cupom.PercentualDesconto : 0;

            return Ok(new
            {
                Id = cart.Id,
                PrecoTotal = cart.PrecoTotal,
                PrecoTotalDesconto = cart.PrecoTotalDesconto,
                Items = GenerateSubTotalData(items, descountToCalculateSubItem)
            });
        }

        [HttpPost(template: "Cart")]
        public async Task<IActionResult> CartAsync([FromServices] AppDbContext context, [FromBody] NovoCarrinhoViewModel model)
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
                HttpContext.Response.Cookies.Delete(CookieTokenCart);//Será que devo remover do Response e adicionar de novo? Como atualizar?
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

        [HttpPut(template: "Cart/{id}")]
        public async Task<IActionResult> PutAsync([FromServices] AppDbContext context, [FromBody] AtualizaCarrinhoViewModel model, [FromRoute] int id)
        {
            if (!ModelState.IsValid || !model.IsValidEntryData())
                return BadRequest(model.Notifications);

            var cart = await context.Carrinhos.Include(nn => nn.CarrinhoItems).FirstOrDefaultAsync(item => item.Id == id);

            if (cart == null)
                return NotFound("Carrinho não encontrado!");

            cart.Cupom = model.Cupom;
            cart.UpdatePrices();

            cart.Usuario = model.Usuario;
            cart.Ativo = model.Ativo;

            try
            {
                await context.SaveChangesAsync();

                return Ok(cart);
            }
            catch (Exception er)
            {
                return BadRequest(er.Message);
            }
        }

        [HttpDelete(template: "Cart/{id}")]
        public async Task<IActionResult> DeleteAsync([FromServices] AppDbContext context, [FromRoute] int id)
        {
            var cart = await context.Carrinhos.Include(nn => nn.CarrinhoItems).FirstOrDefaultAsync(item => item.Id == id);
            if (cart == null)
                return NotFound("Carrinho não encontrado!");

            try
            {
                if (cart.CarrinhoItems?.Count > 0)
                {
                    var cartItemsController = new CartItemsController();

                    foreach (var item in cart.CarrinhoItems)
                    {
                        await cartItemsController.DeleteAsync(context, item.Id);
                    }
                }

                if (cart.Token == HttpContext.Request.Cookies[CookieTokenCart]?.ToString())
                    HttpContext.Response.Cookies.Delete(CookieTokenCart);

                context.Carrinhos.Remove(cart);
                await context.SaveChangesAsync();

                return Ok();
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
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
        private async Task DeactiveCartsByCookieToken(AppDbContext context, string oldCookie)
        {
            var activeCarts = context.Carrinhos.Where(nn => nn.Ativo && nn.Token == oldCookie);
            if (activeCarts.Any())
            {
                await activeCarts.ForEachAsync(cart => cart.Ativo = false);
            }
        }

        private IEnumerable<ItemTotal> GenerateSubTotalData(IQueryable<CarrinhoItem> items, decimal cartDescount)
        {
            foreach (var item in items)
            {
                yield return new ItemTotal
                {
                    Produto = item.Produto,
                    PrecoTotalItem = item.PrecoTotalItem,
                    PrecoTotalItemDesconto = cartDescount <= 0 ? item.PrecoTotalItem : (item.PrecoTotalItem - ((item.PrecoTotalItem * cartDescount) / 100))
                };
            }
        }

        class ItemTotal
        {
            public Produto Produto { get; set; }
            public decimal PrecoTotalItem { get; set; }
            public decimal PrecoTotalItemDesconto { get; set; }
        }
    }


}

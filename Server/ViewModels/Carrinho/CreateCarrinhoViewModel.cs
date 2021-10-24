using Flunt.Notifications;
using Flunt.Validations;
using Server.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Server.ViewModels.Carrinho
{
    public class CreateCarrinhoViewModel : Notifiable<Notification>
    {
        public Usuario Usuario { get; set; }
        [Required]
        public bool Ativo { get; set; }
        public string Token { get; set; }

        public Models.Carrinho MapTo()
        {
            AddNotifications(new Contract<Notification>().Requires());


            return new Models.Carrinho
            {
                Usuario = Usuario,
                Ativo = Ativo,
                Token = String.IsNullOrEmpty(Token) ? Guid.NewGuid().ToString() : Token,
                PrecoTotal = 0,
                PrecoTotalDesconto = 0,
            };
        }
    }
}

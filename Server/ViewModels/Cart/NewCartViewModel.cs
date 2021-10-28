using Flunt.Notifications;
using Flunt.Validations;
using Server.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Server.ViewModels.Cart
{
    public class NewCartViewModel : Notifiable<Notification>
    {
        public Usuario Usuario { get; set; }
        
        public Models.Carrinho MapTo()
        {
            AddNotifications(new Contract<Notification>().Requires());

            return new Models.Carrinho
            {
                Usuario = Usuario,
                Ativo = true,
                Token = Guid.NewGuid().ToString(),                
            };
        }
    }
}

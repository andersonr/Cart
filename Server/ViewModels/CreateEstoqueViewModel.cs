using Server.Models;
using System.ComponentModel.DataAnnotations;
using Flunt;
using Flunt.Notifications;
using Flunt.Validations;

namespace Server.ViewModels
{
    public class CreateEstoqueViewModel : Notifiable<Notification>
    {
        [Required]
        public int Quantidade { get; set; } = 0;
        [Required]
        public Produto Produto { get; set; }

        public Estoque MapTo()
        {
            AddNotifications(new Contract<Notification>().Requires().IsNotNull(Produto, "Produto não informado!"));

            return new Estoque { Produto = this.Produto, Quantidade = this.Quantidade };
        }
    }
}

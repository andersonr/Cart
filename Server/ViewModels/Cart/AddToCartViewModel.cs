using Flunt.Notifications;
using Flunt.Validations;
using Server.Models;
using System.ComponentModel.DataAnnotations;

namespace Server.ViewModels.Cart
{
    public class AddToCartViewModel : Notifiable<Notification>
    {
        [Required]
        public Carrinho Carrinho { get; set; }
        [Required]
        public Produto Produto { get; set; }
        [Required]
        public long Quantidade { get; set; }

        public bool IsValidData()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsGreaterThan(Quantidade, 0, "Não é possível informar uma quantidade igual ou inferior a zero"));

            return IsValid;
        }
    }
}

using Flunt.Notifications;
using Flunt.Validations;
using System.ComponentModel.DataAnnotations;

namespace Server.ViewModels.Cart
{
    public class UpdateCartItemViewModel : Notifiable<Notification>
    {
        [Required]
        public long Quantidade { get; set; }

        public bool IsEntradasValidas()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsGreaterOrEqualsThan(Quantidade, 0, "Não é possível selecionar uma quantidade negativa"));

            return IsValid;
        }
    }
}

using Flunt.Notifications;
using Flunt.Validations;
using System.ComponentModel.DataAnnotations;

namespace Server.ViewModels.Cart
{
    public class AddToCartViewModel : Notifiable<Notification>
    {
        [Required]
        public int ProdutoID { get; set; }
        [Required]
        public long Qtdade { get; set; }

        public bool IsEntradasValidas()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsGreaterOrEqualsThan(Qtdade, 0, "Não é possível selecionar uma quantidade negativa")
                .IsGreaterThan(ProdutoID, 0, "Produto não foi selecionado"));

            return IsValid;
        }
    }
}

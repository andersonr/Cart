using Flunt.Notifications;
using Flunt.Validations;
using Server.Models;
using System.ComponentModel.DataAnnotations;

namespace Server.ViewModels.Cart
{
    public class UpdateCartViewModel : Notifiable<Notification>
    {
        public Cupom Cupom { get; set; }
        public bool Ativo { get; set; }
        public Usuario Usuario { get; set; }

        public bool IsValidEntryData()
        {
            AddNotifications(new Contract<Notification>().Requires().IsGreaterThan(Cupom.PercentualDesconto, 0, "O desconto não pode ser Zero porcento!"));
            //.IsGreaterOrEqualsThan(PrecoTotal, 0, "Preço total não pode ser negativo!")
            //.IsGreaterOrEqualsThan(PrecoTotalDesconto, 0, "Preço com desconto não pode ser negativo!")
            //.IsLowerOrEqualsThan(PrecoTotalDesconto,PrecoTotal, "O preço com desconto não pode ser maior que o preço total!"));

            return IsValid;
        }
    }
}

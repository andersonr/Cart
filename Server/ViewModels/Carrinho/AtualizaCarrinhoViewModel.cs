using Flunt.Notifications;
using Flunt.Validations;
using Server.Models;
using System.ComponentModel.DataAnnotations;

namespace Server.ViewModels.Carrinho
{
    public class AtualizaCarrinhoViewModel : Notifiable<Notification>
    {
        public Cupom Cupom { get; set; }
        public bool Ativo { get; set; }
        public Usuario Usuario { get; set; }

        public bool IsValidEntryData()
        {
            AddNotifications(new Contract<Notification>()
                        .Requires());
            //.IsGreaterThan(Cupom.PercentualDesconto, 0, "O desconto não pode ser Zero porcento!"));

            if (Cupom != null && Cupom.PercentualDesconto <= 0)
                AddNotification("Percentual de desconto é zero", "O percentual de desconto do cupom não pode ser zero");

            return IsValid;
        }
    }
}

using Flunt.Notifications;
using Flunt.Validations;
using System.ComponentModel.DataAnnotations;

namespace Server.ViewModels
{
    public class CreateCupomViewModel : Notifiable<Notification>
    {
        [Required]
        public bool IsAtivo { get; set; } = true;
        [Required]
        public string Codigo { get; set; }
        [Required]
        public decimal PercentualDesconto { get; set; }

        public bool IsValidData()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsGreaterThan(Codigo, 4, "O código deve ter ao menos 4 caracteres!")
                .IsBetween(PercentualDesconto, 1, 100, "O percentual de desconto informado não é válido!"));

            return IsValid;
        }
    }
}

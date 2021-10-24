using Flunt.Notifications;
using Flunt.Validations;
using Server.Models;
using System.ComponentModel.DataAnnotations;

namespace Server.ViewModels
{
    public class CreateProdutoViewModel : Notifiable<Notification>
    {
        [Required]
        public bool IsDisponivel { get; set; } = false;
        [Required]
        public decimal PrecoUnitario { get; set; }
        [Required]
        public string Nome { get; set; }

        public Produto MapTo()
        {
            AddNotifications(new Contract<Notification>().Requires().IsGreaterOrEqualsThan(PrecoUnitario, 0, "O preço unitário não pode ser negativo!"));

            return new Produto
            {
                IsDisponivel = IsDisponivel,
                PrecoUnitario = PrecoUnitario,
                Nome = Nome
            };
        }
    }
}

using Flunt.Notifications;
using Flunt.Validations;
using Server.Models;
using System.ComponentModel.DataAnnotations;

namespace Server.ViewModels
{
    public class CreateUsuarioViewModel : Notifiable<Notification>
    {
        [Required]
        public string Nome { get; set; }

        public Usuario MapTo()
        {
            AddNotifications(new Contract<Notification>().Requires().IsNotNull(Nome, "O nome do usuário não pode ser vazio!"));

            return new Usuario
            {
                Nome = Nome
            };
        }
    }
}

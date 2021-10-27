using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }

        public ICollection<CarrinhoUsuarioFavorito> CarrinhoUsuarioFavoritos { get; set; }
        public ICollection<Carrinho> Carrinhos{ get; set; }

    }
}

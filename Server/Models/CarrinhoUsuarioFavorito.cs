using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class CarrinhoUsuarioFavorito
    {
        public int Id { get; set; }
        [Required]
        public Usuario Usuario { get; set; }
        [Required]
        public Carrinho Carrinho { get; set; }
    }
}

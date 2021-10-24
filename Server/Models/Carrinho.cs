using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class Carrinho
    {
        public int Id { get; set; }
        [Required]
        public decimal PrecoTotal { get; set; }
        [Required]
        public decimal PrecoTotalDesconto { get; set; }
        public Cupom Cupom { get; set; }
        [Required]
        [MinLength(5)]
        public string Token { get; set; }
        public Usuario Usuario { get; set; }
        [Required]
        public bool Ativo { get; set; }
    }
}

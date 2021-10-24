using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class CarrinhoItem
    {
        public int Id { get; set; }
        [Required]
        public Produto Produto { get; set; }
        [Required]        
        public Carrinho Carrinho { get; set; }
        [Required]
        public long Quantidade { get; set; }
        [Required]
        public decimal PrecoTotalItem { get; set; }
    }
}

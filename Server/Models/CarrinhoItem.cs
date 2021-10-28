using System;
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
        [Range(0, Int64.MaxValue, ErrorMessage = "Não é possível um valor negativo como quantidade!")]
        public long Quantidade { get; set; }
        [Required]
        public decimal PrecoTotalItem { get; private set; }

        internal void UpdatePrices()
        {
            PrecoTotalItem = Quantidade * Produto.PrecoUnitario;
        }
    }
}

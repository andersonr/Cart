using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;

namespace Server.Models
{
    public class Carrinho
    {
        public int Id { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Não é possível um valor negativo como preço total!")]
        public decimal PrecoTotal { get; private set; } = decimal.Zero;
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Não é possível um valor negativo como preço total!")]
        public decimal PrecoTotalDesconto { get; private set; } = decimal.Zero;
        [JsonIgnore]
        public int? CupomId { get; set; }
        public Cupom Cupom { get; set; }
        [Required]
        [MinLength(5)]
        public string Token { get; set; }
        [JsonIgnore]
        public int? UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        [Required]
        public bool Ativo { get; set; }

        [JsonIgnore]
        public ICollection<CarrinhoItem> CarrinhoItems { get; set; }
        [JsonIgnore]
        public ICollection<CarrinhoUsuarioFavorito> CarrinhoUsuarioFavoritos { get; set; }

        public void UpdatePrices()
        {
            PrecoTotal = CarrinhoItems != null ? CarrinhoItems.Sum(item => item.PrecoTotalItem) : 0;
            if (PrecoTotal > 0 && Cupom != null && Cupom.PercentualDesconto > 0)
                PrecoTotalDesconto = PrecoTotal - ((PrecoTotal * Cupom.PercentualDesconto) / 100);
            else
                PrecoTotalDesconto = PrecoTotal;
        }
    }
}

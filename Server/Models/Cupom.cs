using System;
using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class Cupom
    {
        public int Id { get; set; }
        [Required]
        [MinLength(5)]
        public string Codigo { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Não é possível um valor negativo como percentual de desconto!")]
        public decimal PercentualDesconto { get; set; }
        [Required]
        public bool IsAtivo { get; set; }        
    }
}

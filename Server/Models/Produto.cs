using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class Produto
    {
        public int Id { get; set; }
        [Required]
        public bool IsDisponivel { get; set; }
        [Required]
        public decimal PrecoUnitario { get; set; }
        [Required]
        public string Nome { get; set; }
    }
}

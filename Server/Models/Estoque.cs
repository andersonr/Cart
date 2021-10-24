using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class Estoque
    {
        public int Id { get; set; }
        [Required]
        public int Quantidade { get; set; }
        [Required]
        public Produto Produto { get; set; }
    }
}

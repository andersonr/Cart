using System.ComponentModel.DataAnnotations;

namespace Server.ViewModels
{
    public class CreateCupomViewModel
    {
        [Required]
        public string Codigo { get; set; }
        [Required]
        public decimal PercentualDesconto { get; set; }
    }
}

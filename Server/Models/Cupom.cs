using System;

namespace Server.Models
{
    public class Cupom
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public decimal PercentualDesconto { get; set; }
        public bool IsAtivo { get; set; }        
    }
}

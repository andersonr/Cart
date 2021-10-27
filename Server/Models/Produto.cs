using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Server.Models
{

    public class Produto
    {
        public int Id { get; set; }
        [Required]
        public bool IsDisponivel { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Não é possível inserir um valor negativo como preço unitário!")]
        public decimal PrecoUnitario { get; set; }
        [Required]
        public string Nome { get; set; }

        [JsonIgnore]
        public ICollection<CarrinhoItem> CarrinhoItems { get; set; }

        //public override bool Equals(object obj)
        //{
        //    if (obj == null)
        //        return false;

        //    try
        //    {
        //        Produto other = obj as Produto;
        //        if (other == null)
        //            return false;
        //        else
        //            return this.IsDisponivel == other.IsDisponivel &&
        //                this.PrecoUnitario == other.PrecoUnitario && 
        //                this.Id == other.Id && this.Nome == other.Nome;
        //    }
        //    catch (System.Exception)
        //    {
        //        return false;
        //    }
        //}
    }
}

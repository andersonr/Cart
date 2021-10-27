using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Server.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }

        [JsonIgnore]
        public ICollection<CarrinhoUsuarioFavorito> CarrinhoUsuarioFavoritos { get; set; }
        [JsonIgnore]
        public ICollection<Carrinho> Carrinhos{ get; set; }

    }
}

using Server.Models;
using Microsoft.EntityFrameworkCore;

namespace Test
{
    public class FakeDBContext : DbContext
    {
        public virtual DbSet<Cupom> Cupons { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<Estoque> Estoques { get; set; }
        public virtual DbSet<Produto> Produtos { get; set; }
        public virtual DbSet<Carrinho> Carrinhos { get; set; }
        public virtual DbSet<CarrinhoItem> CarrinhoItems { get; set; }
        public virtual DbSet<CarrinhoUsuarioFavorito> CarrinhoUsuarioFavoritos { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Models.Cupom> Cupons { get; set; }
        public DbSet<Models.Usuario> Usuarios { get; set; }
        public DbSet<Models.Estoque> Estoques { get; set; }
        public DbSet<Models.Produto> Produtos { get; set; }
        public DbSet<Models.Carrinho> Carrinhos { get; set; }
        public DbSet<Models.CarrinhoItem> CarrinhoItems { get; set; }
        public DbSet<Models.CarrinhoUsuarioFavorito> CarrinhoUsuarioFavoritos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite("DataSource=app.db;Cache=Shared");
    }
}

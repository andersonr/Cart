using Microsoft.EntityFrameworkCore;

namespace Server.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Models.Cupom> Cupons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite("DataSource=app.db;Cache=Shared");
    }
}

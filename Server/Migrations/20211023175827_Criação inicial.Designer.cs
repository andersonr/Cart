// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Server.Data;

namespace Server.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20211023175827_Criação inicial")]
    partial class Criaçãoinicial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.11");

            modelBuilder.Entity("Server.Models.Cupom", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Codigo")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsAtivo")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("PercentualDesconto")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Cupons");
                });
#pragma warning restore 612, 618
        }
    }
}

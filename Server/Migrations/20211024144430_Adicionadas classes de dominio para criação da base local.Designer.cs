﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Server.Data;

namespace Server.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20211024144430_Adicionadas classes de dominio para criação da base local")]
    partial class Adicionadasclassesdedominioparacriaçãodabaselocal
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.11");

            modelBuilder.Entity("Server.Models.Carrinho", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Ativo")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CupomId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("PrecoTotal")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("PrecoTotalDesconto")
                        .HasColumnType("TEXT");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("UsuarioId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CupomId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Carrinhos");
                });

            modelBuilder.Entity("Server.Models.CarrinhoItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CarrinhoId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("PrecoTotalItem")
                        .HasColumnType("TEXT");

                    b.Property<int?>("ProdutoId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("Quantidade")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CarrinhoId");

                    b.HasIndex("ProdutoId");

                    b.ToTable("CarrinhoItems");
                });

            modelBuilder.Entity("Server.Models.CarrinhoUsuarioFavorito", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CarrinhoId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("UsuarioId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CarrinhoId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("CarrinhoUsuarioFavoritos");
                });

            modelBuilder.Entity("Server.Models.Cupom", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Codigo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsAtivo")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("PercentualDesconto")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Cupons");
                });

            modelBuilder.Entity("Server.Models.Estoque", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ProdutoId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Quantidade")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ProdutoId");

                    b.ToTable("Estoques");
                });

            modelBuilder.Entity("Server.Models.Produto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDisponivel")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("PrecoUnitario")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Produtos");
                });

            modelBuilder.Entity("Server.Models.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("Server.Models.Carrinho", b =>
                {
                    b.HasOne("Server.Models.Cupom", "Cupom")
                        .WithMany()
                        .HasForeignKey("CupomId");

                    b.HasOne("Server.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("UsuarioId");

                    b.Navigation("Cupom");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Server.Models.CarrinhoItem", b =>
                {
                    b.HasOne("Server.Models.Carrinho", "Carrinho")
                        .WithMany()
                        .HasForeignKey("CarrinhoId");

                    b.HasOne("Server.Models.Produto", "Produto")
                        .WithMany()
                        .HasForeignKey("ProdutoId");

                    b.Navigation("Carrinho");

                    b.Navigation("Produto");
                });

            modelBuilder.Entity("Server.Models.CarrinhoUsuarioFavorito", b =>
                {
                    b.HasOne("Server.Models.Carrinho", "Carrinho")
                        .WithMany()
                        .HasForeignKey("CarrinhoId");

                    b.HasOne("Server.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("UsuarioId");

                    b.Navigation("Carrinho");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Server.Models.Estoque", b =>
                {
                    b.HasOne("Server.Models.Produto", "Produto")
                        .WithMany()
                        .HasForeignKey("ProdutoId");

                    b.Navigation("Produto");
                });
#pragma warning restore 612, 618
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Server.Migrations
{
    public partial class Adicionadaestruturadecarrinhosfavoritos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarrinhoItems_Carrinhos_CarrinhoId",
                table: "CarrinhoItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CarrinhoItems_Produtos_ProdutoId",
                table: "CarrinhoItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CarrinhoUsuarioFavoritos_Carrinhos_CarrinhoId",
                table: "CarrinhoUsuarioFavoritos");

            migrationBuilder.DropForeignKey(
                name: "FK_CarrinhoUsuarioFavoritos_Usuarios_UsuarioId",
                table: "CarrinhoUsuarioFavoritos");

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioId",
                table: "CarrinhoUsuarioFavoritos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CarrinhoId",
                table: "CarrinhoUsuarioFavoritos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProdutoId",
                table: "CarrinhoItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CarrinhoId",
                table: "CarrinhoItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CarrinhoItems_Carrinhos_CarrinhoId",
                table: "CarrinhoItems",
                column: "CarrinhoId",
                principalTable: "Carrinhos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarrinhoItems_Produtos_ProdutoId",
                table: "CarrinhoItems",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarrinhoUsuarioFavoritos_Carrinhos_CarrinhoId",
                table: "CarrinhoUsuarioFavoritos",
                column: "CarrinhoId",
                principalTable: "Carrinhos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarrinhoUsuarioFavoritos_Usuarios_UsuarioId",
                table: "CarrinhoUsuarioFavoritos",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarrinhoItems_Carrinhos_CarrinhoId",
                table: "CarrinhoItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CarrinhoItems_Produtos_ProdutoId",
                table: "CarrinhoItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CarrinhoUsuarioFavoritos_Carrinhos_CarrinhoId",
                table: "CarrinhoUsuarioFavoritos");

            migrationBuilder.DropForeignKey(
                name: "FK_CarrinhoUsuarioFavoritos_Usuarios_UsuarioId",
                table: "CarrinhoUsuarioFavoritos");

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioId",
                table: "CarrinhoUsuarioFavoritos",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "CarrinhoId",
                table: "CarrinhoUsuarioFavoritos",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "ProdutoId",
                table: "CarrinhoItems",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "CarrinhoId",
                table: "CarrinhoItems",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_CarrinhoItems_Carrinhos_CarrinhoId",
                table: "CarrinhoItems",
                column: "CarrinhoId",
                principalTable: "Carrinhos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CarrinhoItems_Produtos_ProdutoId",
                table: "CarrinhoItems",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CarrinhoUsuarioFavoritos_Carrinhos_CarrinhoId",
                table: "CarrinhoUsuarioFavoritos",
                column: "CarrinhoId",
                principalTable: "Carrinhos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CarrinhoUsuarioFavoritos_Usuarios_UsuarioId",
                table: "CarrinhoUsuarioFavoritos",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

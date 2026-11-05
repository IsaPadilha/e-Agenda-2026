using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eAgenda.Infraestrutura.Compartilhado.Orm.Migrations
{
    /// <inheritdoc />
    public partial class UsuarioId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TBCategorias_Titulo",
                table: "TBCategorias");

            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioId",
                table: "TBTarefas",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioId",
                table: "TBItensTarefa",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioId",
                table: "TBDespesas",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioId",
                table: "TBCategorias",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioId",
                table: "Contatos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioId",
                table: "Compromisso",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TBCategorias_UsuarioId_Titulo",
                table: "TBCategorias",
                columns: new[] { "UsuarioId", "Titulo" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TBCategorias_UsuarioId_Titulo",
                table: "TBCategorias");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "TBTarefas");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "TBItensTarefa");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "TBDespesas");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "TBCategorias");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Contatos");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Compromisso");

            migrationBuilder.CreateIndex(
                name: "IX_TBCategorias_Titulo",
                table: "TBCategorias",
                column: "Titulo",
                unique: true);
        }
    }
}

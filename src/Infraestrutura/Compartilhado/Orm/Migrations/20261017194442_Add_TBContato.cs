using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eAgenda.Infraestrutura.Compartilhado.Orm
{
    /// <inheritdoc />
    public partial class Add_TBContato : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TBItensTarefa_TBTarefas_TarefaId",
                table: "TBItensTarefa");

            migrationBuilder.AddForeignKey(
                name: "FK_TBItensTarefa_TBTarefas_TarefaId",
                table: "TBItensTarefa",
                column: "TarefaId",
                principalTable: "TBTarefas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TBItensTarefa_TBTarefas_TarefaId",
                table: "TBItensTarefa");

            migrationBuilder.AddForeignKey(
                name: "FK_TBItensTarefa_TBTarefas_TarefaId",
                table: "TBItensTarefa",
                column: "TarefaId",
                principalTable: "TBTarefas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eAgenda.Infraestrutura.Compartilhado.Orm
{
    /// <inheritdoc />
    public partial class Add_TBDespesas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TBDespesas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Descricao = table.Column<string>(type: "varchar(100)", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FormaPagamento = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBDespesas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TBDespesasCategorias",
                columns: table => new
                {
                    DespesasId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoriasId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBDespesasCategorias", x => new { x.DespesasId, x.CategoriasId });
                    table.ForeignKey(
                        name: "FK_TBDespesasCategorias_TBDespesas_DespesasId",
                        column: x => x.DespesasId,
                        principalTable: "TBDespesas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TBDespesasCategorias_TBCategorias_CategoriasId",
                        column: x => x.CategoriasId,
                        principalTable: "TBCategorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TBDespesasCategorias_CategoriasId",
                table: "TBDespesasCategorias",
                column: "CategoriasId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF OBJECT_ID('TBDespesasCategorias', 'U') IS NOT NULL DROP TABLE TBDespesasCategorias;
                IF OBJECT_ID('TBDespesaCategoria', 'U') IS NOT NULL DROP TABLE TBDespesaCategoria;
                IF OBJECT_ID('TBDespesas', 'U') IS NOT NULL DROP TABLE TBDespesas;
            ");
        }
    }
}
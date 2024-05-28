using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroServicesCuponAPI.Migrations
{
    /// <inheritdoc />
    public partial class cratetablecupon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cupon",
                columns: table => new
                {
                    IdCupon = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descuento = table.Column<double>(type: "float", nullable: false),
                    CantidadMinima = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cupon", x => x.IdCupon);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cupon");
        }
    }
}

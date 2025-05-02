using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibreriaWeb.Migrations
{
    /// <inheritdoc />
    public partial class NuevoAtributo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagenRuta",
                table: "Libros",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagenRuta",
                table: "Libros");
        }
    }
}

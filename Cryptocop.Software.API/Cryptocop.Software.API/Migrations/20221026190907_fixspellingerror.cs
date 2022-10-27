using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cryptocop.Software.API.Migrations
{
    public partial class fixspellingerror : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HouseNumbert",
                table: "Address",
                newName: "HouseNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HouseNumber",
                table: "Address",
                newName: "HouseNumbert");
        }
    }
}

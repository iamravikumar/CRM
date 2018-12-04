using Microsoft.EntityFrameworkCore.Migrations;

namespace CRM.Migrations
{
    public partial class AddSomeAttributeToFirms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Firms",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Firms",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Province",
                table: "Firms",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Firms");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Firms");

            migrationBuilder.DropColumn(
                name: "Province",
                table: "Firms");
        }
    }
}

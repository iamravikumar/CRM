using Microsoft.EntityFrameworkCore.Migrations;

namespace CRM.Migrations
{
    public partial class ChangeDivisionAttribute : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DivisionType",
                table: "Firms");

            migrationBuilder.AddColumn<string>(
                name: "Division",
                table: "Firms",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Division",
                table: "Firms");

            migrationBuilder.AddColumn<int>(
                name: "DivisionType",
                table: "Firms",
                nullable: false,
                defaultValue: 0);
        }
    }
}

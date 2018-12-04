using Microsoft.EntityFrameworkCore.Migrations;

namespace CRM.Migrations
{
    public partial class DeleteAttributeFromFirms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Website",
                table: "Firms",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Division",
                table: "Firms",
                maxLength: 1,
                nullable: false,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Website",
                table: "Firms",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Division",
                table: "Firms",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 1);
        }
    }
}

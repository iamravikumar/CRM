using Microsoft.EntityFrameworkCore.Migrations;

namespace CRM.Migrations
{
    public partial class RemoveInstallmentFromPaymentOption : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Installment",
                table: "PaymentOptions");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PaymentOptions",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PaymentOptions",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Installment",
                table: "PaymentOptions",
                nullable: false,
                defaultValue: 0);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CRM.Migrations
{
    public partial class ModifiedPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentOn",
                table: "Payments",
                newName: "UpdatedAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDay",
                table: "Payments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentDay",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Payments",
                newName: "PaymentOn");
        }
    }
}

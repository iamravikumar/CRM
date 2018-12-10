using Microsoft.EntityFrameworkCore.Migrations;

namespace CRM.Migrations
{
    public partial class AddUserIDToSchedules : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "Schedules",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_UserID",
                table: "Schedules",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_AspNetUsers_UserID",
                table: "Schedules",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_AspNetUsers_UserID",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_UserID",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Schedules");
        }
    }
}

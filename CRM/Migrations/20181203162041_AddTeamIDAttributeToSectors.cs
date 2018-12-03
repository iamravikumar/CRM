using Microsoft.EntityFrameworkCore.Migrations;

namespace CRM.Migrations
{
    public partial class AddTeamIDAttributeToSectors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeamID",
                table: "Sectors",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Sectors_TeamID",
                table: "Sectors",
                column: "TeamID");

            migrationBuilder.AddForeignKey(
                name: "FK_Sectors_Teams_TeamID",
                table: "Sectors",
                column: "TeamID",
                principalTable: "Teams",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sectors_Teams_TeamID",
                table: "Sectors");

            migrationBuilder.DropIndex(
                name: "IX_Sectors_TeamID",
                table: "Sectors");

            migrationBuilder.DropColumn(
                name: "TeamID",
                table: "Sectors");
        }
    }
}

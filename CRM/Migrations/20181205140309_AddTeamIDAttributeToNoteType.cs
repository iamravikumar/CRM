using Microsoft.EntityFrameworkCore.Migrations;

namespace CRM.Migrations
{
    public partial class AddTeamIDAttributeToNoteType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeamID",
                table: "NoteTypes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NoteTypes_TeamID",
                table: "NoteTypes",
                column: "TeamID");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteTypes_Teams_TeamID",
                table: "NoteTypes",
                column: "TeamID",
                principalTable: "Teams",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NoteTypes_Teams_TeamID",
                table: "NoteTypes");

            migrationBuilder.DropIndex(
                name: "IX_NoteTypes_TeamID",
                table: "NoteTypes");

            migrationBuilder.DropColumn(
                name: "TeamID",
                table: "NoteTypes");
        }
    }
}

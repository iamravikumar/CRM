using Microsoft.EntityFrameworkCore.Migrations;

namespace CRM.Migrations
{
    public partial class FixedBugsOnMessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_WriterID",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "WriterID",
                table: "Messages",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_WriterID",
                table: "Messages",
                newName: "IX_Messages_UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_UserID",
                table: "Messages",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_UserID",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Messages",
                newName: "WriterID");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_UserID",
                table: "Messages",
                newName: "IX_Messages_WriterID");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_WriterID",
                table: "Messages",
                column: "WriterID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

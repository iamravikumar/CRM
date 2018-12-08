using Microsoft.EntityFrameworkCore.Migrations;

namespace CRM.Migrations
{
    public partial class AddParentAttributeToMessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentID",
                table: "Messages",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ParentID",
                table: "Messages",
                column: "ParentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Messages_ParentID",
                table: "Messages",
                column: "ParentID",
                principalTable: "Messages",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Messages_ParentID",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ParentID",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ParentID",
                table: "Messages");
        }
    }
}

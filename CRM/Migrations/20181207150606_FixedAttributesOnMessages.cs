using Microsoft.EntityFrameworkCore.Migrations;

namespace CRM.Migrations
{
    public partial class FixedAttributesOnMessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Messages_MessageID",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_MessageID",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "MessageID",
                table: "Messages");

            migrationBuilder.AddColumn<bool>(
                name: "IsViewed",
                table: "Messages",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsViewed",
                table: "Messages");

            migrationBuilder.AddColumn<int>(
                name: "MessageID",
                table: "Messages",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_MessageID",
                table: "Messages",
                column: "MessageID");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Messages_MessageID",
                table: "Messages",
                column: "MessageID",
                principalTable: "Messages",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

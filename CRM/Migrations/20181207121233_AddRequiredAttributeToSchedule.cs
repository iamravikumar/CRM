using Microsoft.EntityFrameworkCore.Migrations;

namespace CRM.Migrations
{
    public partial class AddRequiredAttributeToSchedule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Personnels_PersonnelID",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Programmes_ProgrammeID",
                table: "Schedules");

            migrationBuilder.AlterColumn<int>(
                name: "ProgrammeID",
                table: "Schedules",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PersonnelID",
                table: "Schedules",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Personnels_PersonnelID",
                table: "Schedules",
                column: "PersonnelID",
                principalTable: "Personnels",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Programmes_ProgrammeID",
                table: "Schedules",
                column: "ProgrammeID",
                principalTable: "Programmes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Personnels_PersonnelID",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Programmes_ProgrammeID",
                table: "Schedules");

            migrationBuilder.AlterColumn<int>(
                name: "ProgrammeID",
                table: "Schedules",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "PersonnelID",
                table: "Schedules",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Personnels_PersonnelID",
                table: "Schedules",
                column: "PersonnelID",
                principalTable: "Personnels",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Programmes_ProgrammeID",
                table: "Schedules",
                column: "ProgrammeID",
                principalTable: "Programmes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

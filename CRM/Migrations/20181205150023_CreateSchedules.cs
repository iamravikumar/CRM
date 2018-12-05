using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CRM.Migrations
{
    public partial class CreateSchedules : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PersonnelID = table.Column<int>(nullable: false),
                    TeamID = table.Column<int>(nullable: true),
                    ProgrammeID = table.Column<int>(nullable: true),
                    StartedAt = table.Column<DateTime>(nullable: false),
                    FinishedAt = table.Column<DateTime>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Schedules_Personnels_PersonnelID",
                        column: x => x.PersonnelID,
                        principalTable: "Personnels",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Schedules_Programmes_ProgrammeID",
                        column: x => x.ProgrammeID,
                        principalTable: "Programmes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Schedules_Teams_TeamID",
                        column: x => x.TeamID,
                        principalTable: "Teams",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_PersonnelID",
                table: "Schedules",
                column: "PersonnelID");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_ProgrammeID",
                table: "Schedules",
                column: "ProgrammeID");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_TeamID",
                table: "Schedules",
                column: "TeamID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Schedules");
        }
    }
}

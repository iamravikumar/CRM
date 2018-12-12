using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CRM.Migrations
{
    public partial class CreateServicesPaymentsAndPaymentOptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentOptions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TeamID = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Ratio = table.Column<double>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentOptions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PaymentOptions_Teams_TeamID",
                        column: x => x.TeamID,
                        principalTable: "Teams",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TeamID = table.Column<int>(nullable: true),
                    ProductID = table.Column<int>(nullable: true),
                    PersonnelID = table.Column<int>(nullable: true),
                    PaymentOptionID = table.Column<int>(nullable: false),
                    Total = table.Column<decimal>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Services_PaymentOptions_PaymentOptionID",
                        column: x => x.PaymentOptionID,
                        principalTable: "PaymentOptions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Services_Personnels_PersonnelID",
                        column: x => x.PersonnelID,
                        principalTable: "Personnels",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Services_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Services_Teams_TeamID",
                        column: x => x.TeamID,
                        principalTable: "Teams",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TeamID = table.Column<int>(nullable: true),
                    ServiceID = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Payments_Services_ServiceID",
                        column: x => x.ServiceID,
                        principalTable: "Services",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payments_Teams_TeamID",
                        column: x => x.TeamID,
                        principalTable: "Teams",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentOptions_TeamID",
                table: "PaymentOptions",
                column: "TeamID");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ServiceID",
                table: "Payments",
                column: "ServiceID");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_TeamID",
                table: "Payments",
                column: "TeamID");

            migrationBuilder.CreateIndex(
                name: "IX_Services_PaymentOptionID",
                table: "Services",
                column: "PaymentOptionID");

            migrationBuilder.CreateIndex(
                name: "IX_Services_PersonnelID",
                table: "Services",
                column: "PersonnelID");

            migrationBuilder.CreateIndex(
                name: "IX_Services_ProductID",
                table: "Services",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Services_TeamID",
                table: "Services",
                column: "TeamID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "PaymentOptions");
        }
    }
}

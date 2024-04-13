using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileToEmailLinker.Migrations
{
    /// <inheritdoc />
    public partial class AlertEntityAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FixedDatesSchedulation_MailingPlanId",
                table: "FixedDatesSchedulation");

            migrationBuilder.CreateTable(
                name: "Alert",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    DateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    References = table.Column<string>(type: "TEXT", nullable: true),
                    AlertSeverity = table.Column<int>(type: "INTEGER", nullable: false),
                    Visualized = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alert", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FixedDatesSchedulation_MailingPlanId",
                table: "FixedDatesSchedulation",
                column: "MailingPlanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alert");

            migrationBuilder.DropIndex(
                name: "IX_FixedDatesSchedulation_MailingPlanId",
                table: "FixedDatesSchedulation");

            migrationBuilder.CreateIndex(
                name: "IX_FixedDatesSchedulation_MailingPlanId",
                table: "FixedDatesSchedulation",
                column: "MailingPlanId",
                unique: true);
        }
    }
}

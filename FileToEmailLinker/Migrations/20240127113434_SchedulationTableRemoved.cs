using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileToEmailLinker.Migrations
{
    /// <inheritdoc />
    public partial class SchedulationTableRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MailingPlan_Schedulation_SchedulationId",
                table: "MailingPlan");

            migrationBuilder.DropTable(
                name: "Schedulation");

            migrationBuilder.DropIndex(
                name: "IX_MailingPlan_SchedulationId",
                table: "MailingPlan");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Schedulation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    EndDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Friday = table.Column<bool>(type: "INTEGER", nullable: false),
                    Monday = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Recurrence = table.Column<int>(type: "INTEGER", nullable: false),
                    Saturday = table.Column<bool>(type: "INTEGER", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Sunday = table.Column<bool>(type: "INTEGER", nullable: false),
                    Thursday = table.Column<bool>(type: "INTEGER", nullable: false),
                    Time = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    Tuesday = table.Column<bool>(type: "INTEGER", nullable: false),
                    Wednesday = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedulation", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MailingPlan_SchedulationId",
                table: "MailingPlan",
                column: "SchedulationId");

            migrationBuilder.AddForeignKey(
                name: "FK_MailingPlan_Schedulation_SchedulationId",
                table: "MailingPlan",
                column: "SchedulationId",
                principalTable: "Schedulation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

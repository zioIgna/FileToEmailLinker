using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileToEmailLinker.Migrations
{
    /// <inheritdoc />
    public partial class MainClassesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MailingPlanId",
                table: "Receiver",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FileRef",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileRef", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Schedulation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Recurrence = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Time = table.Column<TimeOnly>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedulation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MailingPlan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ActiveState = table.Column<int>(type: "INTEGER", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: false),
                    SchedulationId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailingPlan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MailingPlan_Schedulation_SchedulationId",
                        column: x => x.SchedulationId,
                        principalTable: "Schedulation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FileRefMailingPlan",
                columns: table => new
                {
                    FileNamesId = table.Column<int>(type: "INTEGER", nullable: false),
                    MailingPlanListId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileRefMailingPlan", x => new { x.FileNamesId, x.MailingPlanListId });
                    table.ForeignKey(
                        name: "FK_FileRefMailingPlan_FileRef_FileNamesId",
                        column: x => x.FileNamesId,
                        principalTable: "FileRef",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FileRefMailingPlan_MailingPlan_MailingPlanListId",
                        column: x => x.MailingPlanListId,
                        principalTable: "MailingPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Receiver_MailingPlanId",
                table: "Receiver",
                column: "MailingPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_FileRefMailingPlan_MailingPlanListId",
                table: "FileRefMailingPlan",
                column: "MailingPlanListId");

            migrationBuilder.CreateIndex(
                name: "IX_MailingPlan_SchedulationId",
                table: "MailingPlan",
                column: "SchedulationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Receiver_MailingPlan_MailingPlanId",
                table: "Receiver",
                column: "MailingPlanId",
                principalTable: "MailingPlan",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Receiver_MailingPlan_MailingPlanId",
                table: "Receiver");

            migrationBuilder.DropTable(
                name: "FileRefMailingPlan");

            migrationBuilder.DropTable(
                name: "FileRef");

            migrationBuilder.DropTable(
                name: "MailingPlan");

            migrationBuilder.DropTable(
                name: "Schedulation");

            migrationBuilder.DropIndex(
                name: "IX_Receiver_MailingPlanId",
                table: "Receiver");

            migrationBuilder.DropColumn(
                name: "MailingPlanId",
                table: "Receiver");
        }
    }
}

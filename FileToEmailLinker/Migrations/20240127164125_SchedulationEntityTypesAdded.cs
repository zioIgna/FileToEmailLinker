using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileToEmailLinker.Migrations
{
    /// <inheritdoc />
    public partial class SchedulationEntityTypesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SchedulationId",
                table: "MailingPlan");

            migrationBuilder.CreateTable(
                name: "FixedDatesSchedulation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DatesList = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    Time = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    MailingPlanId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FixedDatesSchedulation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FixedDatesSchedulation_MailingPlan_MailingPlanId",
                        column: x => x.MailingPlanId,
                        principalTable: "MailingPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonthlySchedulation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    One = table.Column<bool>(type: "INTEGER", nullable: false),
                    Two = table.Column<bool>(type: "INTEGER", nullable: false),
                    Three = table.Column<bool>(type: "INTEGER", nullable: false),
                    Four = table.Column<bool>(type: "INTEGER", nullable: false),
                    Five = table.Column<bool>(type: "INTEGER", nullable: false),
                    Six = table.Column<bool>(type: "INTEGER", nullable: false),
                    Seven = table.Column<bool>(type: "INTEGER", nullable: false),
                    Eight = table.Column<bool>(type: "INTEGER", nullable: false),
                    Nine = table.Column<bool>(type: "INTEGER", nullable: false),
                    Ten = table.Column<bool>(type: "INTEGER", nullable: false),
                    Eleven = table.Column<bool>(type: "INTEGER", nullable: false),
                    Twelve = table.Column<bool>(type: "INTEGER", nullable: false),
                    Thirteen = table.Column<bool>(type: "INTEGER", nullable: false),
                    Fourteen = table.Column<bool>(type: "INTEGER", nullable: false),
                    Fifteen = table.Column<bool>(type: "INTEGER", nullable: false),
                    Sixteen = table.Column<bool>(type: "INTEGER", nullable: false),
                    Seventeen = table.Column<bool>(type: "INTEGER", nullable: false),
                    Eighteen = table.Column<bool>(type: "INTEGER", nullable: false),
                    Nineteen = table.Column<bool>(type: "INTEGER", nullable: false),
                    Twenty = table.Column<bool>(type: "INTEGER", nullable: false),
                    Twentyone = table.Column<bool>(type: "INTEGER", nullable: false),
                    Twentytwo = table.Column<bool>(type: "INTEGER", nullable: false),
                    Twentythree = table.Column<bool>(type: "INTEGER", nullable: false),
                    Twentyfour = table.Column<bool>(type: "INTEGER", nullable: false),
                    Twentyfive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Twentysix = table.Column<bool>(type: "INTEGER", nullable: false),
                    Twentyseven = table.Column<bool>(type: "INTEGER", nullable: false),
                    Twentyeight = table.Column<bool>(type: "INTEGER", nullable: false),
                    Twentynine = table.Column<bool>(type: "INTEGER", nullable: false),
                    Thirty = table.Column<bool>(type: "INTEGER", nullable: false),
                    Thirtyone = table.Column<bool>(type: "INTEGER", nullable: false),
                    January = table.Column<bool>(type: "INTEGER", nullable: false),
                    February = table.Column<bool>(type: "INTEGER", nullable: false),
                    March = table.Column<bool>(type: "INTEGER", nullable: false),
                    April = table.Column<bool>(type: "INTEGER", nullable: false),
                    May = table.Column<bool>(type: "INTEGER", nullable: false),
                    June = table.Column<bool>(type: "INTEGER", nullable: false),
                    July = table.Column<bool>(type: "INTEGER", nullable: false),
                    August = table.Column<bool>(type: "INTEGER", nullable: false),
                    September = table.Column<bool>(type: "INTEGER", nullable: false),
                    October = table.Column<bool>(type: "INTEGER", nullable: false),
                    November = table.Column<bool>(type: "INTEGER", nullable: false),
                    December = table.Column<bool>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    Time = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    MailingPlanId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlySchedulation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonthlySchedulation_MailingPlan_MailingPlanId",
                        column: x => x.MailingPlanId,
                        principalTable: "MailingPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeeklySchedulation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Monday = table.Column<bool>(type: "INTEGER", nullable: false),
                    Tuesday = table.Column<bool>(type: "INTEGER", nullable: false),
                    Wednesday = table.Column<bool>(type: "INTEGER", nullable: false),
                    Thursday = table.Column<bool>(type: "INTEGER", nullable: false),
                    Friday = table.Column<bool>(type: "INTEGER", nullable: false),
                    Saturday = table.Column<bool>(type: "INTEGER", nullable: false),
                    Sunday = table.Column<bool>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    Time = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    MailingPlanId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklySchedulation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeeklySchedulation_MailingPlan_MailingPlanId",
                        column: x => x.MailingPlanId,
                        principalTable: "MailingPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FixedDatesSchedulation_MailingPlanId",
                table: "FixedDatesSchedulation",
                column: "MailingPlanId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonthlySchedulation_MailingPlanId",
                table: "MonthlySchedulation",
                column: "MailingPlanId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WeeklySchedulation_MailingPlanId",
                table: "WeeklySchedulation",
                column: "MailingPlanId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FixedDatesSchedulation");

            migrationBuilder.DropTable(
                name: "MonthlySchedulation");

            migrationBuilder.DropTable(
                name: "WeeklySchedulation");

            migrationBuilder.AddColumn<int>(
                name: "SchedulationId",
                table: "MailingPlan",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}

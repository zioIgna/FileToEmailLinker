using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileToEmailLinker.Migrations
{
    /// <inheritdoc />
    public partial class DaysOfWeekAddedToSchedulation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Friday",
                table: "Schedulation",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Monday",
                table: "Schedulation",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Saturday",
                table: "Schedulation",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Sunday",
                table: "Schedulation",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Thursday",
                table: "Schedulation",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Tuesday",
                table: "Schedulation",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Wednesday",
                table: "Schedulation",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Friday",
                table: "Schedulation");

            migrationBuilder.DropColumn(
                name: "Monday",
                table: "Schedulation");

            migrationBuilder.DropColumn(
                name: "Saturday",
                table: "Schedulation");

            migrationBuilder.DropColumn(
                name: "Sunday",
                table: "Schedulation");

            migrationBuilder.DropColumn(
                name: "Thursday",
                table: "Schedulation");

            migrationBuilder.DropColumn(
                name: "Tuesday",
                table: "Schedulation");

            migrationBuilder.DropColumn(
                name: "Wednesday",
                table: "Schedulation");
        }
    }
}

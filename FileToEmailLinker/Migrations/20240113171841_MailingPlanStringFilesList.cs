using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileToEmailLinker.Migrations
{
    /// <inheritdoc />
    public partial class MailingPlanStringFilesList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileRefMailingPlan");

            migrationBuilder.AddColumn<string>(
                name: "FileStringList",
                table: "MailingPlan",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileStringList",
                table: "MailingPlan");

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
                name: "IX_FileRefMailingPlan_MailingPlanListId",
                table: "FileRefMailingPlan",
                column: "MailingPlanListId");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileToEmailLinker.Migrations
{
    /// <inheritdoc />
    public partial class ReceiverCanHaveMultipleMailingPlans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Receiver_MailingPlan_MailingPlanId",
                table: "Receiver");

            migrationBuilder.DropIndex(
                name: "IX_Receiver_MailingPlanId",
                table: "Receiver");

            migrationBuilder.DropColumn(
                name: "MailingPlanId",
                table: "Receiver");

            migrationBuilder.CreateTable(
                name: "MailingPlanReceiver",
                columns: table => new
                {
                    MailingPlanListId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReceiverListId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailingPlanReceiver", x => new { x.MailingPlanListId, x.ReceiverListId });
                    table.ForeignKey(
                        name: "FK_MailingPlanReceiver_MailingPlan_MailingPlanListId",
                        column: x => x.MailingPlanListId,
                        principalTable: "MailingPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MailingPlanReceiver_Receiver_ReceiverListId",
                        column: x => x.ReceiverListId,
                        principalTable: "Receiver",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MailingPlanReceiver_ReceiverListId",
                table: "MailingPlanReceiver",
                column: "ReceiverListId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MailingPlanReceiver");

            migrationBuilder.AddColumn<int>(
                name: "MailingPlanId",
                table: "Receiver",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Receiver_MailingPlanId",
                table: "Receiver",
                column: "MailingPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Receiver_MailingPlan_MailingPlanId",
                table: "Receiver",
                column: "MailingPlanId",
                principalTable: "MailingPlan",
                principalColumn: "Id");
        }
    }
}

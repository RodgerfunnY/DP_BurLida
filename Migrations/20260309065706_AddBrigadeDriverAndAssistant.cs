using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DP_BurLida.Migrations
{
    /// <inheritdoc />
    public partial class AddBrigadeDriverAndAssistant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubstituteArrangementAssistantMaster",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "SubstituteArrangementDriver",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "SubstituteDrillingAssistantMaster",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "SubstituteDrillingDriver",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "BrigadeComposition",
                table: "BrigadeModelData");

            migrationBuilder.RenameColumn(
                name: "AssistantMaster",
                table: "BrigadeModelData",
                newName: "DrillingMasterAssistant");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DrillingMasterAssistant",
                table: "BrigadeModelData",
                newName: "AssistantMaster");

            migrationBuilder.AddColumn<string>(
                name: "SubstituteArrangementAssistantMaster",
                table: "Order",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubstituteArrangementDriver",
                table: "Order",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubstituteDrillingAssistantMaster",
                table: "Order",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubstituteDrillingDriver",
                table: "Order",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BrigadeComposition",
                table: "BrigadeModelData",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }
    }
}

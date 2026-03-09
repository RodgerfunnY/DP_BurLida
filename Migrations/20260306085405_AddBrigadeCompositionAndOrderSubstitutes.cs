using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DP_BurLida.Migrations
{
    /// <inheritdoc />
    public partial class AddBrigadeCompositionAndOrderSubstitutes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssistantMaster",
                table: "BrigadeModelData",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BrigadeComposition",
                table: "BrigadeModelData",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Driver",
                table: "BrigadeModelData",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

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

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Guarded drops: some environments had this migration applied with empty Up().
            migrationBuilder.Sql(@"
IF COL_LENGTH('BrigadeModelData', 'AssistantMaster') IS NOT NULL
    ALTER TABLE [BrigadeModelData] DROP COLUMN [AssistantMaster];

IF COL_LENGTH('BrigadeModelData', 'BrigadeComposition') IS NOT NULL
    ALTER TABLE [BrigadeModelData] DROP COLUMN [BrigadeComposition];

IF COL_LENGTH('BrigadeModelData', 'Driver') IS NOT NULL
    ALTER TABLE [BrigadeModelData] DROP COLUMN [Driver];

IF COL_LENGTH('[Order]', 'SubstituteArrangementAssistantMaster') IS NOT NULL
    ALTER TABLE [Order] DROP COLUMN [SubstituteArrangementAssistantMaster];

IF COL_LENGTH('[Order]', 'SubstituteArrangementDriver') IS NOT NULL
    ALTER TABLE [Order] DROP COLUMN [SubstituteArrangementDriver];

IF COL_LENGTH('[Order]', 'SubstituteDrillingAssistantMaster') IS NOT NULL
    ALTER TABLE [Order] DROP COLUMN [SubstituteDrillingAssistantMaster];

IF COL_LENGTH('[Order]', 'SubstituteDrillingDriver') IS NOT NULL
    ALTER TABLE [Order] DROP COLUMN [SubstituteDrillingDriver];
");

        }
    }
}

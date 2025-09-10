using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DP_BurLida.Migrations
{
    /// <inheritdoc />
    public partial class SkladBD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrigadeModelData_UserModelData_ResponsibleUserId",
                table: "BrigadeModelData");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Заказы",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "SkladModelData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameSubjecte = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Info = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkladModelData", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_BrigadeModelData_UserModelData_ResponsibleUserId",
                table: "BrigadeModelData",
                column: "ResponsibleUserId",
                principalTable: "UserModelData",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrigadeModelData_UserModelData_ResponsibleUserId",
                table: "BrigadeModelData");

            migrationBuilder.DropTable(
                name: "SkladModelData");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Заказы");

            migrationBuilder.AddForeignKey(
                name: "FK_BrigadeModelData_UserModelData_ResponsibleUserId",
                table: "BrigadeModelData",
                column: "ResponsibleUserId",
                principalTable: "UserModelData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

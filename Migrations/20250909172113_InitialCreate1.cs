using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DP_BurLida.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrigadeModelData_UserModelData_ResponsibleUserId",
                table: "BrigadeModelData");

            migrationBuilder.AlterColumn<int>(
                name: "ResponsibleUserId",
                table: "BrigadeModelData",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

            migrationBuilder.AlterColumn<int>(
                name: "ResponsibleUserId",
                table: "BrigadeModelData",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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

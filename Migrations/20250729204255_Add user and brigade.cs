using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DP_BurLida.Migrations
{
    /// <inheritdoc />
    public partial class Adduserandbrigade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Пользователи",
                table: "Пользователи");

            migrationBuilder.DropColumn(
                name: "Пароль",
                table: "Пользователи");

            migrationBuilder.RenameTable(
                name: "Пользователи",
                newName: "UserModelData");

            migrationBuilder.RenameColumn(
                name: "Фамилия",
                table: "UserModelData",
                newName: "Surname");

            migrationBuilder.RenameColumn(
                name: "Номер телефона",
                table: "UserModelData",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "Имя",
                table: "UserModelData",
                newName: "Name");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "UserModelData",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Surname",
                table: "UserModelData",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "UserModelData",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(14)",
                oldMaxLength: 14);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UserModelData",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserModelData",
                table: "UserModelData",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "BrigadeModelData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameBrigade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Technic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Info = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrigadeModelData", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrigadeModelData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserModelData",
                table: "UserModelData");

            migrationBuilder.RenameTable(
                name: "UserModelData",
                newName: "Пользователи");

            migrationBuilder.RenameColumn(
                name: "Surname",
                table: "Пользователи",
                newName: "Фамилия");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Пользователи",
                newName: "Номер телефона");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Пользователи",
                newName: "Имя");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Пользователи",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Фамилия",
                table: "Пользователи",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Номер телефона",
                table: "Пользователи",
                type: "nvarchar(14)",
                maxLength: 14,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Имя",
                table: "Пользователи",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Пароль",
                table: "Пользователи",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Пользователи",
                table: "Пользователи",
                column: "Id");
        }
    }
}

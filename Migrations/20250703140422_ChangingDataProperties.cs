using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DP_BurLida.Migrations
{
    /// <inheritdoc />
    public partial class ChangingDataProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserModelsData",
                table: "UserModelsData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderModelData",
                table: "OrderModelData");

            migrationBuilder.RenameTable(
                name: "UserModelsData",
                newName: "Пользователи");

            migrationBuilder.RenameTable(
                name: "OrderModelData",
                newName: "Заказы");

            migrationBuilder.RenameColumn(
                name: "Surname",
                table: "Пользователи",
                newName: "Фамилия");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Пользователи",
                newName: "Номер телефона");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Пользователи",
                newName: "Пароль");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Пользователи",
                newName: "Имя");

            migrationBuilder.RenameColumn(
                name: "SurnameClient",
                table: "Заказы",
                newName: "Фамилия");

            migrationBuilder.RenameColumn(
                name: "Pump",
                table: "Заказы",
                newName: "Насос с монтажом");

            migrationBuilder.RenameColumn(
                name: "PricePerMeter",
                table: "Заказы",
                newName: "Цена за метр");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Заказы",
                newName: "Номер телефона");

            migrationBuilder.RenameColumn(
                name: "NameClient",
                table: "Заказы",
                newName: "Имя");

            migrationBuilder.RenameColumn(
                name: "Info",
                table: "Заказы",
                newName: "Дополнительная информация");

            migrationBuilder.RenameColumn(
                name: "District",
                table: "Заказы",
                newName: "Район");

            migrationBuilder.RenameColumn(
                name: "Diameter",
                table: "Заказы",
                newName: "Диаметр скважины");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "Заказы",
                newName: "Населеный пункт");

            migrationBuilder.RenameColumn(
                name: "Arrangement",
                table: "Заказы",
                newName: "Обустройство");

            migrationBuilder.RenameColumn(
                name: "Area",
                table: "Заказы",
                newName: "Область");

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
                name: "Пароль",
                table: "Пользователи",
                type: "nvarchar(100)",
                maxLength: 100,
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
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Фамилия",
                table: "Заказы",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Номер телефона",
                table: "Заказы",
                type: "nvarchar(14)",
                maxLength: 14,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Имя",
                table: "Заказы",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Дополнительная информация",
                table: "Заказы",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Район",
                table: "Заказы",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Населеный пункт",
                table: "Заказы",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Обустройство",
                table: "Заказы",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Область",
                table: "Заказы",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Пользователи",
                table: "Пользователи",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Заказы",
                table: "Заказы",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Пользователи",
                table: "Пользователи");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Заказы",
                table: "Заказы");

            migrationBuilder.RenameTable(
                name: "Пользователи",
                newName: "UserModelsData");

            migrationBuilder.RenameTable(
                name: "Заказы",
                newName: "OrderModelData");

            migrationBuilder.RenameColumn(
                name: "Фамилия",
                table: "UserModelsData",
                newName: "Surname");

            migrationBuilder.RenameColumn(
                name: "Пароль",
                table: "UserModelsData",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "Номер телефона",
                table: "UserModelsData",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "Имя",
                table: "UserModelsData",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Цена за метр",
                table: "OrderModelData",
                newName: "PricePerMeter");

            migrationBuilder.RenameColumn(
                name: "Фамилия",
                table: "OrderModelData",
                newName: "SurnameClient");

            migrationBuilder.RenameColumn(
                name: "Район",
                table: "OrderModelData",
                newName: "District");

            migrationBuilder.RenameColumn(
                name: "Обустройство",
                table: "OrderModelData",
                newName: "Arrangement");

            migrationBuilder.RenameColumn(
                name: "Область",
                table: "OrderModelData",
                newName: "Area");

            migrationBuilder.RenameColumn(
                name: "Номер телефона",
                table: "OrderModelData",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "Насос с монтажом",
                table: "OrderModelData",
                newName: "Pump");

            migrationBuilder.RenameColumn(
                name: "Населеный пункт",
                table: "OrderModelData",
                newName: "City");

            migrationBuilder.RenameColumn(
                name: "Имя",
                table: "OrderModelData",
                newName: "NameClient");

            migrationBuilder.RenameColumn(
                name: "Дополнительная информация",
                table: "OrderModelData",
                newName: "Info");

            migrationBuilder.RenameColumn(
                name: "Диаметр скважины",
                table: "OrderModelData",
                newName: "Diameter");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "UserModelsData",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Surname",
                table: "UserModelsData",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "UserModelsData",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "UserModelsData",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(14)",
                oldMaxLength: 14);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UserModelsData",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "SurnameClient",
                table: "OrderModelData",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "District",
                table: "OrderModelData",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Arrangement",
                table: "OrderModelData",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Area",
                table: "OrderModelData",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "OrderModelData",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(14)",
                oldMaxLength: 14);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "OrderModelData",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "NameClient",
                table: "OrderModelData",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Info",
                table: "OrderModelData",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserModelsData",
                table: "UserModelsData",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderModelData",
                table: "OrderModelData",
                column: "Id");
        }
    }
}

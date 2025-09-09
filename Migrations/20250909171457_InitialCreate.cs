using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DP_BurLida.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Заказы",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Имя = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Фамилия = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Номертелефона = table.Column<string>(name: "Номер телефона", type: "nvarchar(14)", maxLength: 14, nullable: false),
                    Область = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Район = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Населеныйпункт = table.Column<string>(name: "Населеный пункт", type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Диаметрскважины = table.Column<int>(name: "Диаметр скважины", type: "int", maxLength: 4, nullable: false),
                    Ценазаметр = table.Column<int>(name: "Цена за метр", type: "int", maxLength: 4, nullable: false),
                    Насоссмонтажом = table.Column<int>(name: "Насос с монтажом", type: "int", maxLength: 5, nullable: false),
                    Обустройство = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Дополнительнаяинформация = table.Column<string>(name: "Дополнительная информация", type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Датасоздания = table.Column<DateTime>(name: "Дата создания", type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Заказы", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserModelData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserModelData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BrigadeModelData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameBrigade = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Technic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Info = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ResponsibleUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrigadeModelData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrigadeModelData_UserModelData_ResponsibleUserId",
                        column: x => x.ResponsibleUserId,
                        principalTable: "UserModelData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrigadeModelData_ResponsibleUserId",
                table: "BrigadeModelData",
                column: "ResponsibleUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Заказы");

            migrationBuilder.DropTable(
                name: "BrigadeModelData");

            migrationBuilder.DropTable(
                name: "UserModelData");
        }
    }
}

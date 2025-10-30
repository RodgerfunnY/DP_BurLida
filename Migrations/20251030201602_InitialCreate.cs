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
                    ResponsibleUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrigadeModelData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrigadeModelData_UserModelData_ResponsibleUserId",
                        column: x => x.ResponsibleUserId,
                        principalTable: "UserModelData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameClient = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    SurnameClient = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    Area = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    District = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Diameter = table.Column<int>(type: "int", maxLength: 4, nullable: false),
                    PricePerMeter = table.Column<int>(type: "int", maxLength: 4, nullable: false),
                    Pump = table.Column<int>(type: "int", maxLength: 5, nullable: false),
                    Arrangement = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Info = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationTimeData = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    WorkDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DrillingBrigadeId = table.Column<int>(type: "int", nullable: true),
                    ArrangementBrigadeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_BrigadeModelData_ArrangementBrigadeId",
                        column: x => x.ArrangementBrigadeId,
                        principalTable: "BrigadeModelData",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Order_BrigadeModelData_DrillingBrigadeId",
                        column: x => x.DrillingBrigadeId,
                        principalTable: "BrigadeModelData",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrigadeModelData_ResponsibleUserId",
                table: "BrigadeModelData",
                column: "ResponsibleUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ArrangementBrigadeId",
                table: "Order",
                column: "ArrangementBrigadeId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_DrillingBrigadeId",
                table: "Order",
                column: "DrillingBrigadeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "SkladModelData");

            migrationBuilder.DropTable(
                name: "BrigadeModelData");

            migrationBuilder.DropTable(
                name: "UserModelData");
        }
    }
}

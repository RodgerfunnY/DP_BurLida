using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DP_BurLida.Migrations
{
    /// <inheritdoc />
    public partial class InitialMain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeviceToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserEmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Token = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    Platform = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceToken", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    RecipientEmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderInstallmentPaymentStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    SlotKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderInstallmentPaymentStatus", x => x.Id);
                });

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
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false)
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
                    Driver = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DrillingMasterAssistant = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
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
                    City = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Diameter = table.Column<int>(type: "int", maxLength: 4, nullable: false),
                    PricePerMeter = table.Column<int>(type: "int", maxLength: 4, nullable: false),
                    Pump = table.Column<int>(type: "int", maxLength: 5, nullable: false),
                    MetersCount = table.Column<int>(type: "int", nullable: true),
                    Depth = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    StaticLevel = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DynamicLevel = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Filter = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PumpModel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Arrangement = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PumpInstalled = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ArrangementDone = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsDrillingInstallment = table.Column<bool>(type: "bit", nullable: false),
                    DrillingFirstContribution = table.Column<int>(type: "int", nullable: true),
                    DrillingFirstPayment = table.Column<int>(type: "int", nullable: true),
                    DrillingFirstPaymentDueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DrillingSecondPayment = table.Column<int>(type: "int", nullable: true),
                    DrillingSecondPaymentDueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DrillingThirdPayment = table.Column<int>(type: "int", nullable: true),
                    DrillingThirdPaymentDueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DrillingFourthPayment = table.Column<int>(type: "int", nullable: true),
                    DrillingFourthPaymentDueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsArrangementInstallment = table.Column<bool>(type: "bit", nullable: false),
                    ArrangementFirstContribution = table.Column<int>(type: "int", nullable: true),
                    ArrangementFirstPayment = table.Column<int>(type: "int", nullable: true),
                    ArrangementFirstPaymentDueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ArrangementSecondPayment = table.Column<int>(type: "int", nullable: true),
                    ArrangementSecondPaymentDueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ArrangementThirdPayment = table.Column<int>(type: "int", nullable: true),
                    ArrangementThirdPaymentDueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ArrangementFourthPayment = table.Column<int>(type: "int", nullable: true),
                    ArrangementFourthPaymentDueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalDrillingAmount = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TotalArrangementAmount = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    InstallmentEripNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Info = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrigadeStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreationTimeData = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    WorkDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ArrangementDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Contractor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Coordinates = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Sewer = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
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

            migrationBuilder.CreateTable(
                name: "OrderComment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsDone = table.Column<bool>(type: "bit", nullable: false),
                    DoneAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderComment_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrigadeModelData_ResponsibleUserId",
                table: "BrigadeModelData",
                column: "ResponsibleUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceToken_Token",
                table: "DeviceToken",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeviceToken_UserEmail_Platform",
                table: "DeviceToken",
                columns: new[] { "UserEmail", "Platform" });

            migrationBuilder.CreateIndex(
                name: "IX_Notification_OrderId",
                table: "Notification",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_RecipientEmail_IsRead",
                table: "Notification",
                columns: new[] { "RecipientEmail", "IsRead" });

            migrationBuilder.CreateIndex(
                name: "IX_Order_ArrangementBrigadeId",
                table: "Order",
                column: "ArrangementBrigadeId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_DrillingBrigadeId",
                table: "Order",
                column: "DrillingBrigadeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderComment_OrderId",
                table: "OrderComment",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderInstallmentPaymentStatus_OrderId_SlotKey",
                table: "OrderInstallmentPaymentStatus",
                columns: new[] { "OrderId", "SlotKey" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceToken");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "OrderComment");

            migrationBuilder.DropTable(
                name: "OrderInstallmentPaymentStatus");

            migrationBuilder.DropTable(
                name: "SkladModelData");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "BrigadeModelData");

            migrationBuilder.DropTable(
                name: "UserModelData");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DP_BurLida.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderExtraFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // База уже содержит таблицы Order, SkladModelData, BrigadeModelData, UserModelData.
            // В этой миграции добавляем только новые поля в таблицу Order.

            migrationBuilder.AddColumn<int>(
                name: "MetersCount",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PumpModel",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ArrangementDate",
                table: "Order",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Contractor",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Coordinates",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sewer",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MetersCount",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "PumpModel",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ArrangementDate",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Contractor",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Coordinates",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Sewer",
                table: "Order");
        }
    }
}

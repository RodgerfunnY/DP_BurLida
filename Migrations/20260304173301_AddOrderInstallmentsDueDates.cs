using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DP_BurLida.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderInstallmentsDueDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArrangementFirstContribution",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ArrangementFirstPayment",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ArrangementFirstPaymentDueDate",
                table: "Order",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ArrangementFourthPayment",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ArrangementFourthPaymentDueDate",
                table: "Order",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ArrangementSecondPayment",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ArrangementSecondPaymentDueDate",
                table: "Order",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ArrangementThirdPayment",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ArrangementThirdPaymentDueDate",
                table: "Order",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DrillingFirstContribution",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DrillingFirstPayment",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DrillingFirstPaymentDueDate",
                table: "Order",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DrillingFourthPayment",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DrillingFourthPaymentDueDate",
                table: "Order",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DrillingSecondPayment",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DrillingSecondPaymentDueDate",
                table: "Order",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DrillingThirdPayment",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DrillingThirdPaymentDueDate",
                table: "Order",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsArrangementInstallment",
                table: "Order",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDrillingInstallment",
                table: "Order",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArrangementFirstContribution",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ArrangementFirstPayment",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ArrangementFirstPaymentDueDate",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ArrangementFourthPayment",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ArrangementFourthPaymentDueDate",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ArrangementSecondPayment",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ArrangementSecondPaymentDueDate",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ArrangementThirdPayment",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ArrangementThirdPaymentDueDate",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "DrillingFirstContribution",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "DrillingFirstPayment",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "DrillingFirstPaymentDueDate",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "DrillingFourthPayment",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "DrillingFourthPaymentDueDate",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "DrillingSecondPayment",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "DrillingSecondPaymentDueDate",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "DrillingThirdPayment",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "DrillingThirdPaymentDueDate",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "IsArrangementInstallment",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "IsDrillingInstallment",
                table: "Order");
        }
    }
}

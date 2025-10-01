using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changePaymentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Payment");

            migrationBuilder.RenameColumn(
                name: "RawResponse",
                table: "Payment",
                newName: "RetrieveRawResponse");

            migrationBuilder.AddColumn<string>(
                name: "CfRawResponse",
                table: "Payment",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CfRawResponse",
                table: "Payment");

            migrationBuilder.RenameColumn(
                name: "RetrieveRawResponse",
                table: "Payment",
                newName: "RawResponse");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Payment",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Payment",
                type: "datetime(6)",
                nullable: true);
        }
    }
}

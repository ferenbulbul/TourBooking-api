using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class tours : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TourRoutePrices_Tours_TourId",
                table: "TourRoutePrices");

            migrationBuilder.DropIndex(
                name: "IX_TourRoutePrices_TourId",
                table: "TourRoutePrices");

            migrationBuilder.DropColumn(
                name: "TourId",
                table: "TourRoutePrices");

            migrationBuilder.AddColumn<Guid>(
                name: "TourEntityId",
                table: "TourRoutePrices",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_TourRoutePrices_TourEntityId",
                table: "TourRoutePrices",
                column: "TourEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_TourRoutePrices_Tours_TourEntityId",
                table: "TourRoutePrices",
                column: "TourEntityId",
                principalTable: "Tours",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TourRoutePrices_Tours_TourEntityId",
                table: "TourRoutePrices");

            migrationBuilder.DropIndex(
                name: "IX_TourRoutePrices_TourEntityId",
                table: "TourRoutePrices");

            migrationBuilder.DropColumn(
                name: "TourEntityId",
                table: "TourRoutePrices");

            migrationBuilder.AddColumn<Guid>(
                name: "TourId",
                table: "TourRoutePrices",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_TourRoutePrices_TourId",
                table: "TourRoutePrices",
                column: "TourId");

            migrationBuilder.AddForeignKey(
                name: "FK_TourRoutePrices_Tours_TourId",
                table: "TourRoutePrices",
                column: "TourId",
                principalTable: "Tours",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

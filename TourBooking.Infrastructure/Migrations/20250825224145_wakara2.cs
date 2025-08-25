using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class wakara2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_DriverLocation_DriverLocationId",
                table: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_DriverLocationId",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "DriverLocationId",
                table: "Drivers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DriverLocationId",
                table: "Drivers",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_DriverLocationId",
                table: "Drivers",
                column: "DriverLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_DriverLocation_DriverLocationId",
                table: "Drivers",
                column: "DriverLocationId",
                principalTable: "DriverLocation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

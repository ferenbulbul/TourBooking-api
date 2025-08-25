using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class waqd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_AgencyId",
                table: "Bookings",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_DriverId",
                table: "Bookings",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_VehicleId",
                table: "Bookings",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Agencies_AgencyId",
                table: "Bookings",
                column: "AgencyId",
                principalTable: "Agencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Drivers_DriverId",
                table: "Bookings",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Vehicles_VehicleId",
                table: "Bookings",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_DriverLocation_DriverLocationId",
                table: "Drivers",
                column: "DriverLocationId",
                principalTable: "DriverLocation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Agencies_AgencyId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Drivers_DriverId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Vehicles_VehicleId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_DriverLocation_DriverLocationId",
                table: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_DriverLocationId",
                table: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_AgencyId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_DriverId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_VehicleId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "DriverLocationId",
                table: "Drivers");
        }
    }
}

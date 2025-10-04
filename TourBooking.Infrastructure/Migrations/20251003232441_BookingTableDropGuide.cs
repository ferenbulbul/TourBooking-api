using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BookingTableDropGuide : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Guides_GuideUserEntityId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_GuideUserEntityId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "GuideUserEntityId",
                table: "Bookings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GuideUserEntityId",
                table: "Bookings",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_GuideUserEntityId",
                table: "Bookings",
                column: "GuideUserEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Guides_GuideUserEntityId",
                table: "Bookings",
                column: "GuideUserEntityId",
                principalTable: "Guides",
                principalColumn: "Id");
        }
    }
}

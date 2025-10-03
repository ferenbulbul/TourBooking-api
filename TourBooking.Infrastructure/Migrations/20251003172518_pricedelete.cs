using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class pricedelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TourPricings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TourPricings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CityId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CountryId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DistrictId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DriverId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RegionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TourId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    VehicleId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourPricings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TourPricings_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TourPricings_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TourPricings_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TourPricings_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TourPricings_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TourPricings_Tours_TourId",
                        column: x => x.TourId,
                        principalTable: "Tours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TourPricings_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TourPricings_CityId",
                table: "TourPricings",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_TourPricings_CountryId",
                table: "TourPricings",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_TourPricings_DistrictId",
                table: "TourPricings",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_TourPricings_DriverId",
                table: "TourPricings",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_TourPricings_RegionId",
                table: "TourPricings",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_TourPricings_TourId",
                table: "TourPricings",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_TourPricings_VehicleId",
                table: "TourPricings",
                column: "VehicleId");
        }
    }
}

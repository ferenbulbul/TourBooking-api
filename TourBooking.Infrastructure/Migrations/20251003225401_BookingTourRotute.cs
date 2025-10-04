using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BookingTourRotute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            

            


            migrationBuilder.DropIndex(
                name: "IX_Bookings_AgencyId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_DriverId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_GuideId_StartDate_EndDate",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "TourRoutePrices");

            migrationBuilder.DropColumn(
                name: "AgencyId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "FromCityId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "FromDistrictId",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "VehicleId",
                table: "Bookings",
                newName: "TourRoutePriceId");

            migrationBuilder.RenameColumn(
                name: "GuideId",
                table: "Bookings",
                newName: "GuideUserEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_VehicleId",
                table: "Bookings",
                newName: "IX_Bookings_TourRoutePriceId");

            migrationBuilder.AddColumn<Guid>(
                name: "GuideTourPriceId",
                table: "Bookings",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.UpdateData(
                table: "Agencies",
                keyColumn: "TursabUrl",
                keyValue: null,
                column: "TursabUrl",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "TursabUrl",
                table: "Agencies",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Agencies",
                keyColumn: "TaxNumber",
                keyValue: null,
                column: "TaxNumber",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "TaxNumber",
                table: "Agencies",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Agencies",
                keyColumn: "PhoneNumber",
                keyValue: null,
                column: "PhoneNumber",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Agencies",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Agencies",
                keyColumn: "FullAddress",
                keyValue: null,
                column: "FullAddress",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "FullAddress",
                table: "Agencies",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Agencies",
                keyColumn: "Email",
                keyValue: null,
                column: "Email",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Agencies",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Agencies",
                keyColumn: "Country",
                keyValue: null,
                column: "Country",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "Agencies",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Agencies",
                keyColumn: "CompanyName",
                keyValue: null,
                column: "CompanyName",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "CompanyName",
                table: "Agencies",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Agencies",
                keyColumn: "City",
                keyValue: null,
                column: "City",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Agencies",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Agencies",
                keyColumn: "AuthorizedUserLastName",
                keyValue: null,
                column: "AuthorizedUserLastName",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorizedUserLastName",
                table: "Agencies",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Agencies",
                keyColumn: "AuthorizedUserFirstName",
                keyValue: null,
                column: "AuthorizedUserFirstName",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorizedUserFirstName",
                table: "Agencies",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_GuideTourPriceId",
                table: "Bookings",
                column: "GuideTourPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_GuideUserEntityId",
                table: "Bookings",
                column: "GuideUserEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_StartDate_EndDate",
                table: "Bookings",
                columns: new[] { "StartDate", "EndDate" });

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_GuideTourPrices_GuideTourPriceId",
                table: "Bookings",
                column: "GuideTourPriceId",
                principalTable: "GuideTourPrices",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Guides_GuideUserEntityId",
                table: "Bookings",
                column: "GuideUserEntityId",
                principalTable: "Guides",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_TourRoutePrices_TourRoutePriceId",
                table: "Bookings",
                column: "TourRoutePriceId",
                principalTable: "TourRoutePrices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_GuideTourPrices_GuideTourPriceId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Guides_GuideUserEntityId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_TourRoutePrices_TourRoutePriceId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_GuideTourPriceId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_GuideUserEntityId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_StartDate_EndDate",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "GuideTourPriceId",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "TourRoutePriceId",
                table: "Bookings",
                newName: "VehicleId");

            migrationBuilder.RenameColumn(
                name: "GuideUserEntityId",
                table: "Bookings",
                newName: "GuideId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_TourRoutePriceId",
                table: "Bookings",
                newName: "IX_Bookings_VehicleId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "TourRoutePrices",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "AgencyId",
                table: "Bookings",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Bookings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "DriverId",
                table: "Bookings",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "FromCityId",
                table: "Bookings",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "FromDistrictId",
                table: "Bookings",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "TursabUrl",
                table: "Agencies",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TaxNumber",
                table: "Agencies",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Agencies",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "FullAddress",
                table: "Agencies",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Agencies",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "Agencies",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CompanyName",
                table: "Agencies",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Agencies",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorizedUserLastName",
                table: "Agencies",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorizedUserFirstName",
                table: "Agencies",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_AgencyId",
                table: "Bookings",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_DriverId",
                table: "Bookings",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_GuideId_StartDate_EndDate",
                table: "Bookings",
                columns: new[] { "GuideId", "StartDate", "EndDate" });

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
                name: "FK_Bookings_Guides_GuideId",
                table: "Bookings",
                column: "GuideId",
                principalTable: "Guides",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Vehicles_VehicleId",
                table: "Bookings",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

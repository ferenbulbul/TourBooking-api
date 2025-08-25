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
            // 1) FK varsa düşür
            migrationBuilder.Sql(@"
SET @sql := (
  SELECT IF(
    EXISTS(
      SELECT 1
      FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
      WHERE CONSTRAINT_SCHEMA = DATABASE()
        AND TABLE_NAME = 'Drivers'
        AND CONSTRAINT_NAME = 'FK_Drivers_DriverLocation_DriverLocationId'
        AND CONSTRAINT_TYPE = 'FOREIGN KEY'
    ),
    'ALTER TABLE `Drivers` DROP FOREIGN KEY `FK_Drivers_DriverLocation_DriverLocationId`',
    'SELECT 1'
  )
);
PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
");

            // 2) Index varsa düşür
            migrationBuilder.Sql(@"
SET @sql := (
  SELECT IF(
    EXISTS(
      SELECT 1
      FROM INFORMATION_SCHEMA.STATISTICS
      WHERE TABLE_SCHEMA = DATABASE()
        AND TABLE_NAME = 'Drivers'
        AND INDEX_NAME = 'IX_Drivers_DriverLocationId'
    ),
    'ALTER TABLE `Drivers` DROP INDEX `IX_Drivers_DriverLocationId`',
    'SELECT 1'
  )
);
PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
");

            // 3) Kolon varsa düşür
            migrationBuilder.Sql(@"
SET @sql := (
  SELECT IF(
    EXISTS(
      SELECT 1
      FROM INFORMATION_SCHEMA.COLUMNS
      WHERE TABLE_SCHEMA = DATABASE()
        AND TABLE_NAME = 'Drivers'
        AND COLUMN_NAME = 'DriverLocationId'
    ),
    'ALTER TABLE `Drivers` DROP COLUMN `DriverLocationId`',
    'SELECT 1'
  )
);
PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
");
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

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SignatureSuites.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Amenity",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Amenity",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Amenity",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Amenity",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Amenity",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Suite",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "SuiteAmenities",
                keyColumns: new[] { "AmenityId", "SuiteId" },
                keyValues: new object[] { 1, 1 },
                column: "CreatedDate",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "SuiteAmenities",
                keyColumns: new[] { "AmenityId", "SuiteId" },
                keyValues: new object[] { 3, 1 },
                column: "CreatedDate",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "SuiteAmenities",
                keyColumns: new[] { "AmenityId", "SuiteId" },
                keyValues: new object[] { 4, 1 },
                column: "CreatedDate",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "SuiteAmenities",
                keyColumns: new[] { "AmenityId", "SuiteId" },
                keyValues: new object[] { 1, 2 },
                column: "CreatedDate",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "SuiteAmenities",
                keyColumns: new[] { "AmenityId", "SuiteId" },
                keyValues: new object[] { 2, 2 },
                column: "CreatedDate",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "SuiteAmenities",
                keyColumns: new[] { "AmenityId", "SuiteId" },
                keyValues: new object[] { 5, 2 },
                column: "CreatedDate",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Amenity",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 1, 20, 0, 22, 12, 833, DateTimeKind.Utc).AddTicks(8744));

            migrationBuilder.UpdateData(
                table: "Amenity",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 1, 20, 0, 22, 12, 834, DateTimeKind.Utc).AddTicks(114));

            migrationBuilder.UpdateData(
                table: "Amenity",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 1, 20, 0, 22, 12, 834, DateTimeKind.Utc).AddTicks(117));

            migrationBuilder.UpdateData(
                table: "Amenity",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 1, 20, 0, 22, 12, 834, DateTimeKind.Utc).AddTicks(119));

            migrationBuilder.UpdateData(
                table: "Amenity",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2026, 1, 20, 0, 22, 12, 834, DateTimeKind.Utc).AddTicks(120));

            migrationBuilder.UpdateData(
                table: "Suite",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "SuiteAmenities",
                keyColumns: new[] { "AmenityId", "SuiteId" },
                keyValues: new object[] { 1, 1 },
                column: "CreatedDate",
                value: new DateTime(2026, 1, 20, 0, 22, 12, 834, DateTimeKind.Utc).AddTicks(1109));

            migrationBuilder.UpdateData(
                table: "SuiteAmenities",
                keyColumns: new[] { "AmenityId", "SuiteId" },
                keyValues: new object[] { 3, 1 },
                column: "CreatedDate",
                value: new DateTime(2026, 1, 20, 0, 22, 12, 834, DateTimeKind.Utc).AddTicks(1930));

            migrationBuilder.UpdateData(
                table: "SuiteAmenities",
                keyColumns: new[] { "AmenityId", "SuiteId" },
                keyValues: new object[] { 4, 1 },
                column: "CreatedDate",
                value: new DateTime(2026, 1, 20, 0, 22, 12, 834, DateTimeKind.Utc).AddTicks(1931));

            migrationBuilder.UpdateData(
                table: "SuiteAmenities",
                keyColumns: new[] { "AmenityId", "SuiteId" },
                keyValues: new object[] { 1, 2 },
                column: "CreatedDate",
                value: new DateTime(2026, 1, 20, 0, 22, 12, 834, DateTimeKind.Utc).AddTicks(1932));

            migrationBuilder.UpdateData(
                table: "SuiteAmenities",
                keyColumns: new[] { "AmenityId", "SuiteId" },
                keyValues: new object[] { 2, 2 },
                column: "CreatedDate",
                value: new DateTime(2026, 1, 20, 0, 22, 12, 834, DateTimeKind.Utc).AddTicks(1933));

            migrationBuilder.UpdateData(
                table: "SuiteAmenities",
                keyColumns: new[] { "AmenityId", "SuiteId" },
                keyValues: new object[] { 5, 2 },
                column: "CreatedDate",
                value: new DateTime(2026, 1, 20, 0, 22, 12, 834, DateTimeKind.Utc).AddTicks(1934));
        }
    }
}

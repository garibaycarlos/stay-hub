using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SignatureSuites.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUsersAndSuiteTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DeleteData(
                table: "Suite",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Suite",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Suite",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.AlterColumn<decimal>(
                name: "Rate",
                table: "Suite",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Suite",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedDate",
                table: "Amenity",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Amenity",
                columns: new[] { "Id", "CreatedDate", "Description", "Name", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 20, 0, 22, 12, 833, DateTimeKind.Utc).AddTicks(8744), "High-speed wireless internet", "Wi-Fi", null },
                    { 2, new DateTime(2026, 1, 20, 0, 22, 12, 834, DateTimeKind.Utc).AddTicks(114), "Climate-controlled rooms", "Air Conditioning", null },
                    { 3, new DateTime(2026, 1, 20, 0, 22, 12, 834, DateTimeKind.Utc).AddTicks(117), "Exclusive private swimming pool", "Private Pool", null },
                    { 4, new DateTime(2026, 1, 20, 0, 22, 12, 834, DateTimeKind.Utc).AddTicks(119), "Unobstructed ocean views", "Ocean View", null },
                    { 5, new DateTime(2026, 1, 20, 0, 22, 12, 834, DateTimeKind.Utc).AddTicks(120), "24/7 in-room dining", "Room Service", null }
                });

            migrationBuilder.UpdateData(
                table: "Suite",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Details", "ImageUrl", "Name", "Rate", "Sqft", "UpdatedDate" },
                values: new object[] { "Luxury suite with private pool and ocean views.", "https://example.com/suite1.jpg", "Royal Ocean Suite", 600m, 2800, null });

            migrationBuilder.UpdateData(
                table: "Suite",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "Details", "ImageUrl", "Name", "Rate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Elegant suite overlooking the mountains.", "https://example.com/suite2.jpg", "Diamond Mountain Suite", 750m, null });

            migrationBuilder.InsertData(
                table: "SuiteAmenities",
                columns: new[] { "AmenityId", "SuiteId", "CreatedDate" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 1, 20, 0, 22, 12, 834, DateTimeKind.Utc).AddTicks(1109) },
                    { 3, 1, new DateTime(2026, 1, 20, 0, 22, 12, 834, DateTimeKind.Utc).AddTicks(1930) },
                    { 4, 1, new DateTime(2026, 1, 20, 0, 22, 12, 834, DateTimeKind.Utc).AddTicks(1931) },
                    { 1, 2, new DateTime(2026, 1, 20, 0, 22, 12, 834, DateTimeKind.Utc).AddTicks(1932) },
                    { 2, 2, new DateTime(2026, 1, 20, 0, 22, 12, 834, DateTimeKind.Utc).AddTicks(1933) },
                    { 5, 2, new DateTime(2026, 1, 20, 0, 22, 12, 834, DateTimeKind.Utc).AddTicks(1934) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DeleteData(
                table: "SuiteAmenities",
                keyColumns: new[] { "AmenityId", "SuiteId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "SuiteAmenities",
                keyColumns: new[] { "AmenityId", "SuiteId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "SuiteAmenities",
                keyColumns: new[] { "AmenityId", "SuiteId" },
                keyValues: new object[] { 4, 1 });

            migrationBuilder.DeleteData(
                table: "SuiteAmenities",
                keyColumns: new[] { "AmenityId", "SuiteId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "SuiteAmenities",
                keyColumns: new[] { "AmenityId", "SuiteId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "SuiteAmenities",
                keyColumns: new[] { "AmenityId", "SuiteId" },
                keyValues: new object[] { 5, 2 });

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Amenity",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.AlterColumn<double>(
                name: "Rate",
                table: "Suite",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Suite",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedDate",
                table: "Amenity",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Suite",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Details", "ImageUrl", "Name", "Rate", "Sqft", "UpdatedDate" },
                values: new object[] { "Luxurious villa with stunning ocean views and private beach access.", "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa1.jpg", "Royal Villa", 500.0, 2500, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Suite",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "Details", "ImageUrl", "Name", "Rate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Elegant villa with marble interiors and panoramic mountain views.", "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa2.jpg", "Diamond Villa", 750.0, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Suite",
                columns: new[] { "Id", "CreatedDate", "Details", "ImageUrl", "Name", "Occupancy", "Rate", "Sqft", "UpdatedDate" },
                values: new object[,]
                {
                    { 3, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Modern villa featuring an infinity pool and outdoor entertainment area.", "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa3.jpg", "Pool Villa", 4, 350.0, 1800, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, new DateTime(2024, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Premium villa with spa facilities and concierge services.", "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa4.jpg", "Luxury Villa", 10, 900.0, 4000, new DateTime(2024, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Charming villa surrounded by tropical gardens and nature trails.", "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa5.jpg", "Garden Villa", 3, 275.0, 1500, new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }
    }
}

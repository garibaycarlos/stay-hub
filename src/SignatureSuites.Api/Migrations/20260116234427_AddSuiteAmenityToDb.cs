using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SignatureSuites.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddSuiteAmenityToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SuiteAmenities",
                columns: table => new
                {
                    SuiteId = table.Column<int>(type: "int", nullable: false),
                    AmenityId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuiteAmenities", x => new { x.SuiteId, x.AmenityId });
                    table.ForeignKey(
                        name: "FK_SuiteAmenities_Amenity_AmenityId",
                        column: x => x.AmenityId,
                        principalTable: "Amenity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SuiteAmenities_Suite_SuiteId",
                        column: x => x.SuiteId,
                        principalTable: "Suite",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SuiteAmenities_AmenityId",
                table: "SuiteAmenities",
                column: "AmenityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SuiteAmenities");
        }
    }
}

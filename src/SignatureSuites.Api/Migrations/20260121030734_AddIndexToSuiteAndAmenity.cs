using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SignatureSuites.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexToSuiteAndAmenity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Suite_Name",
                table: "Suite",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Amenity_Name",
                table: "Amenity",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Suite_Name",
                table: "Suite");

            migrationBuilder.DropIndex(
                name: "IX_Amenity_Name",
                table: "Amenity");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace ANLASH.Migrations
{
    public partial class AddPerformanceIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Universities indexes
            migrationBuilder.CreateIndex(
                name: "IX_Universities_Slug",
                table: "Universities",
                column: "Slug");

            migrationBuilder.CreateIndex(
                name: "IX_Universities_SlugAr",
                table: "Universities",
                column: "SlugAr");

            migrationBuilder.CreateIndex(
                name: "IX_Universities_IsActive_IsFeatured",
                table: "Universities",
                columns: new[] { "IsActive", "IsFeatured" });

            // UniversityPrograms indexes
            migrationBuilder.CreateIndex(
                name: "IX_UniversityPrograms_UniversityId_IsActive",
                table: "UniversityPrograms",
                columns: new[] { "UniversityId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_UniversityPrograms_Level",
                table: "UniversityPrograms",
                column: "Level");

            migrationBuilder.CreateIndex(
                name: "IX_UniversityPrograms_IsFeatured_IsActive",
                table: "UniversityPrograms",
                columns: new[] { "IsFeatured", "IsActive" });

            // UniversityFAQs indexes
            migrationBuilder.CreateIndex(
                name: "IX_UniversityFAQs_UniversityId_IsPublished",
                table: "UniversityFAQs",
                columns: new[] { "UniversityId", "IsPublished" });

            // UniversityContents indexes
            migrationBuilder.CreateIndex(
                name: "IX_UniversityContents_UniversityId_ContentType",
                table: "UniversityContents",
                columns: new[] { "UniversityId", "ContentType" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Universities indexes
            migrationBuilder.DropIndex(
                name: "IX_Universities_Slug",
                table: "Universities");

            migrationBuilder.DropIndex(
                name: "IX_Universities_SlugAr",
                table: "Universities");

            migrationBuilder.DropIndex(
                name: "IX_Universities_IsActive_IsFeatured",
                table: "Universities");

            // UniversityPrograms indexes
            migrationBuilder.DropIndex(
                name: "IX_UniversityPrograms_UniversityId_IsActive",
                table: "UniversityPrograms");

            migrationBuilder.DropIndex(
                name: "IX_UniversityPrograms_Level",
                table: "UniversityPrograms");

            migrationBuilder.DropIndex(
                name: "IX_UniversityPrograms_IsFeatured_IsActive",
                table: "UniversityPrograms");

            // UniversityFAQs indexes
            migrationBuilder.DropIndex(
                name: "IX_UniversityFAQs_UniversityId_IsPublished",
                table: "UniversityFAQs");

            // UniversityContents indexes
            migrationBuilder.DropIndex(
                name: "IX_UniversityContents_UniversityId_ContentType",
                table: "UniversityContents");
        }
    }
}

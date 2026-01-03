using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANLASH.Migrations
{
    /// <inheritdoc />
    public partial class AddUniversityFAQConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsPublished",
                table: "UniversityFAQs",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "UniversityFAQs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_UniversityFAQs_DisplayOrder",
                table: "UniversityFAQs",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_UniversityFAQs_IsPublished",
                table: "UniversityFAQs",
                column: "IsPublished");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UniversityFAQs_DisplayOrder",
                table: "UniversityFAQs");

            migrationBuilder.DropIndex(
                name: "IX_UniversityFAQs_IsPublished",
                table: "UniversityFAQs");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPublished",
                table: "UniversityFAQs",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "UniversityFAQs",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);
        }
    }
}

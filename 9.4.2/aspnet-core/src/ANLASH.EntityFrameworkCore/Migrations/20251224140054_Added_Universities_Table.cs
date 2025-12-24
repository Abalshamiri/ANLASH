using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANLASH.Migrations
{
    /// <inheritdoc />
    public partial class Added_Universities_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Universities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    WebsiteUrl = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Rating = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WorldRanking = table.Column<int>(type: "int", nullable: true),
                    EstablishmentYear = table.Column<int>(type: "int", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    SlugAr = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Universities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Universities_City",
                table: "Universities",
                column: "City");

            migrationBuilder.CreateIndex(
                name: "IX_Universities_Country",
                table: "Universities",
                column: "Country");

            migrationBuilder.CreateIndex(
                name: "IX_Universities_IsActive",
                table: "Universities",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Universities_IsFeatured",
                table: "Universities",
                column: "IsFeatured");

            migrationBuilder.CreateIndex(
                name: "IX_Universities_Rating",
                table: "Universities",
                column: "Rating");

            migrationBuilder.CreateIndex(
                name: "IX_Universities_Slug",
                table: "Universities",
                column: "Slug",
                unique: true,
                filter: "[Slug] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Universities_SlugAr",
                table: "Universities",
                column: "SlugAr",
                unique: true,
                filter: "[SlugAr] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Universities");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANLASH.Migrations
{
    /// <inheritdoc />
    public partial class AddUniversityPrograms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UniversityPrograms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniversityId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Mode = table.Column<int>(type: "int", nullable: false),
                    Field = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FieldAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DurationYears = table.Column<int>(type: "int", nullable: false),
                    DurationSemesters = table.Column<int>(type: "int", nullable: true),
                    DurationMonths = table.Column<int>(type: "int", nullable: true),
                    TotalCredits = table.Column<int>(type: "int", nullable: true),
                    TuitionFee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CurrencyId = table.Column<int>(type: "int", nullable: true),
                    FeeType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ApplicationFee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ApplicationDeadline = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Requirements = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    RequirementsAr = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    SlugAr = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
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
                    table.PrimaryKey("PK_UniversityPrograms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UniversityPrograms_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_UniversityPrograms_Universities_UniversityId",
                        column: x => x.UniversityId,
                        principalTable: "Universities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UniversityPrograms_CurrencyId",
                table: "UniversityPrograms",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_UniversityPrograms_IsActive",
                table: "UniversityPrograms",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_UniversityPrograms_IsFeatured",
                table: "UniversityPrograms",
                column: "IsFeatured");

            migrationBuilder.CreateIndex(
                name: "IX_UniversityPrograms_Level",
                table: "UniversityPrograms",
                column: "Level");

            migrationBuilder.CreateIndex(
                name: "IX_UniversityPrograms_Mode",
                table: "UniversityPrograms",
                column: "Mode");

            migrationBuilder.CreateIndex(
                name: "IX_UniversityPrograms_UniversityId",
                table: "UniversityPrograms",
                column: "UniversityId");

            migrationBuilder.CreateIndex(
                name: "UQ_UniversityPrograms_Slug",
                table: "UniversityPrograms",
                column: "Slug",
                unique: true,
                filter: "[Slug] IS NOT NULL AND [IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "UQ_UniversityPrograms_SlugAr",
                table: "UniversityPrograms",
                column: "SlugAr",
                unique: true,
                filter: "[SlugAr] IS NOT NULL AND [IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UniversityPrograms");
        }
    }
}

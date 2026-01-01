using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANLASH.Migrations
{
    /// <inheritdoc />
    public partial class Add_LanguageCenters_Module : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LanguageCenters",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    AboutText = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    AboutTextAr = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(10,8)", precision: 10, scale: 8, nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(11,8)", precision: 11, scale: 8, nullable: true),
                    WebsiteUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WhatsApp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LogoBlobId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LogoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CoverImageBlobId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GalleryImages = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    IsAccredited = table.Column<bool>(type: "bit", nullable: false),
                    AccreditationBody = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AccreditationNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AccreditationExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RegistrationSteps = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    RequiredDocuments = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    ProvidesAccommodation = table.Column<bool>(type: "bit", nullable: false),
                    AccommodationTypes = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    AccommodationDetails = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    AccommodationDetailsAr = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    SlugAr = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    MetaDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MetaDescriptionAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    Rating = table.Column<decimal>(type: "decimal(3,2)", precision: 3, scale: 2, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_LanguageCenters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LanguageCenters_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_LanguageCenters_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "LanguageCenterFAQs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageCenterId = table.Column<long>(type: "bigint", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    QuestionAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Answer = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    AnswerAr = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CategoryAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    HelpfulCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    NotHelpfulCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
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
                    table.PrimaryKey("PK_LanguageCenterFAQs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LanguageCenterFAQs_LanguageCenters_LanguageCenterId",
                        column: x => x.LanguageCenterId,
                        principalTable: "LanguageCenters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LanguageCourses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageCenterId = table.Column<long>(type: "bigint", nullable: false),
                    CourseName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    CourseNameAr = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CourseType = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    MinAge = table.Column<int>(type: "int", nullable: true),
                    MaxAge = table.Column<int>(type: "int", nullable: true),
                    ClassSize = table.Column<int>(type: "int", nullable: true),
                    AverageClassSize = table.Column<int>(type: "int", nullable: true),
                    HoursPerWeek = table.Column<int>(type: "int", nullable: true),
                    LessonsPerWeek = table.Column<int>(type: "int", nullable: true),
                    LessonDurationMinutes = table.Column<int>(type: "int", nullable: true),
                    StartDates = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MinDurationWeeks = table.Column<int>(type: "int", nullable: true),
                    MaxDurationWeeks = table.Column<int>(type: "int", nullable: true),
                    IncludesMaterials = table.Column<bool>(type: "bit", nullable: false),
                    IncludesCertificate = table.Column<bool>(type: "bit", nullable: false),
                    CertificateType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IncludesPlacementTest = table.Column<bool>(type: "bit", nullable: false),
                    Highlights = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    HighlightsAr = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    TargetAudience = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TargetAudienceAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
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
                    table.PrimaryKey("PK_LanguageCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LanguageCourses_LanguageCenters_LanguageCenterId",
                        column: x => x.LanguageCenterId,
                        principalTable: "LanguageCenters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoursePricing",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageCourseId = table.Column<long>(type: "bigint", nullable: false),
                    DurationWeeks = table.Column<int>(type: "int", nullable: false),
                    Fee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    FeePerWeek = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RegistrationFee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MaterialsFee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExamFee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    VisaDurationWeeks = table.Column<int>(type: "int", nullable: true),
                    VisaProcessingFee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    HasDiscount = table.Column<bool>(type: "bit", nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FinalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PromotionDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PromotionDescriptionAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsMostPopular = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    NotesAr = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
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
                    table.PrimaryKey("PK_CoursePricing", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoursePricing_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CoursePricing_LanguageCourses_LanguageCourseId",
                        column: x => x.LanguageCourseId,
                        principalTable: "LanguageCourses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoursePricing_CurrencyId",
                table: "CoursePricing",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_CoursePricing_DurationWeeks",
                table: "CoursePricing",
                column: "DurationWeeks");

            migrationBuilder.CreateIndex(
                name: "IX_CoursePricing_IsActive",
                table: "CoursePricing",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_CoursePricing_IsMostPopular",
                table: "CoursePricing",
                column: "IsMostPopular");

            migrationBuilder.CreateIndex(
                name: "IX_CoursePricing_LanguageCourseId",
                table: "CoursePricing",
                column: "LanguageCourseId");

            migrationBuilder.CreateIndex(
                name: "UQ_CoursePricing_CourseId_Duration",
                table: "CoursePricing",
                columns: new[] { "LanguageCourseId", "DurationWeeks" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageCenterFAQs_Category",
                table: "LanguageCenterFAQs",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageCenterFAQs_DisplayOrder",
                table: "LanguageCenterFAQs",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageCenterFAQs_IsFeatured",
                table: "LanguageCenterFAQs",
                column: "IsFeatured");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageCenterFAQs_IsPublished",
                table: "LanguageCenterFAQs",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageCenterFAQs_LanguageCenterId",
                table: "LanguageCenterFAQs",
                column: "LanguageCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageCenters_CityId",
                table: "LanguageCenters",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageCenters_CountryId",
                table: "LanguageCenters",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageCenters_IsAccredited",
                table: "LanguageCenters",
                column: "IsAccredited");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageCenters_IsActive",
                table: "LanguageCenters",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageCenters_IsFeatured",
                table: "LanguageCenters",
                column: "IsFeatured");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageCenters_Rating",
                table: "LanguageCenters",
                column: "Rating");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageCenters_Slug",
                table: "LanguageCenters",
                column: "Slug",
                unique: true,
                filter: "[Slug] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageCenters_SlugAr",
                table: "LanguageCenters",
                column: "SlugAr",
                unique: true,
                filter: "[SlugAr] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageCenters_TenantId",
                table: "LanguageCenters",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageCourses_CourseType",
                table: "LanguageCourses",
                column: "CourseType");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageCourses_IsActive",
                table: "LanguageCourses",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageCourses_IsFeatured",
                table: "LanguageCourses",
                column: "IsFeatured");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageCourses_LanguageCenterId",
                table: "LanguageCourses",
                column: "LanguageCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageCourses_Level",
                table: "LanguageCourses",
                column: "Level");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoursePricing");

            migrationBuilder.DropTable(
                name: "LanguageCenterFAQs");

            migrationBuilder.DropTable(
                name: "LanguageCourses");

            migrationBuilder.DropTable(
                name: "LanguageCenters");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANLASH.Migrations
{
    /// <inheritdoc />
    public partial class EnhanceUniversityWithRebuild : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ========================================
            // STEP 1: Drop FK & Indexes from related tables
            // ========================================
            migrationBuilder.DropForeignKey(
                name: "FK_UniversityFAQs_Universities_UniversityId",
                table: "UniversityFAQs");
            
            migrationBuilder.DropForeignKey(
                name: "FK_UniversityPrograms_Universities_UniversityId",
                table: "UniversityPrograms");
            
            migrationBuilder.DropIndex(
                name: "IX_UniversityFAQs_UniversityId",
                table: "UniversityFAQs");
            
            migrationBuilder.DropIndex(
                name: "IX_UniversityPrograms_UniversityId",
                table: "UniversityPrograms");

            // ========================================
            // STEP 2: Create New Universities Table with long ID
            // ========================================
            migrationBuilder.Sql(@"
                CREATE TABLE [Universities_New] (
                    [Id] bigint IDENTITY(1,1) NOT NULL,
                    [Name] nvarchar(300) NOT NULL,
                    [NameAr] nvarchar(300) NOT NULL,
                    [Description] nvarchar(max) NULL,
                    [DescriptionAr] nvarchar(max) NULL,
                    [Type] int NOT NULL,
                    [LogoUrl] nvarchar(500) NULL,
                    [WebsiteUrl] nvarchar(300) NULL,
                    [Email] nvarchar(200) NULL,
                    [Phone] nvarchar(50) NULL,
                    [Rating] decimal(3, 2) NOT NULL DEFAULT 0,
                    [WorldRanking] int NULL,
                    [EstablishmentYear] int NULL,
                    [Slug] nvarchar(400) NULL,
                    [SlugAr] nvarchar(400) NULL,
                    [IsActive] bit NOT NULL DEFAULT 1,
                    [IsFeatured] bit NOT NULL DEFAULT 0,
                    [DisplayOrder] int NOT NULL DEFAULT 0,
                    [Address] nvarchar(500) NULL,
                    
                    -- New Fields
                    [CountryId] int NULL,
                    [CityId] int NULL,
                    [AboutText] NVARCHAR(MAX) NULL,
                    [AboutTextAr] NVARCHAR(MAX) NULL,
                    [LogoBlobId] uniqueidentifier NULL,
                    [CoverImageBlobId] uniqueidentifier NULL,
                    [MetaDescription] nvarchar(500) NULL,
                    [MetaDescriptionAr] nvarchar(500) NULL,
                    [OfferLetterFee] decimal(18,2) NULL,
                    [IntakeMonths] nvarchar(100) NULL,
                    [TenantId] int NULL,
                    
                    -- Audit Fields
                    [CreationTime] datetime2(7) NOT NULL,
                    [CreatorUserId] bigint NULL,
                    [LastModificationTime] datetime2(7) NULL,
                    [LastModifierUserId] bigint NULL,
                    [IsDeleted] bit NOT NULL DEFAULT 0,
                    [DeleterUserId] bigint NULL,
                    [DeletionTime] datetime2(7) NULL,
                    
                    CONSTRAINT [PK_Universities_New] PRIMARY KEY CLUSTERED ([Id] ASC)
                );
            ");

            // ========================================
            // STEP 3: Copy Data from Old to New Table
            // ========================================
            migrationBuilder.Sql(@"
                SET IDENTITY_INSERT [Universities_New] ON;
                
                INSERT INTO [Universities_New] (
                    [Id], [Name], [NameAr], [Description], [DescriptionAr],
                    [Type], [LogoUrl], [WebsiteUrl], [Email], [Phone],
                    [Rating], [WorldRanking], [EstablishmentYear],
                    [Slug], [SlugAr], [IsActive], [IsFeatured], [DisplayOrder],
                    [Address],
                    [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId],
                    [IsDeleted], [DeleterUserId], [DeletionTime]
                )
                SELECT 
                    [Id], [Name], [NameAr], [Description], [DescriptionAr],
                    [Type], [LogoUrl], [WebsiteUrl], [Email], [Phone],
                    [Rating], [WorldRanking], [EstablishmentYear],
                    [Slug], [SlugAr], [IsActive], [IsFeatured], [DisplayOrder],
                    [Address],
                    [CreationTime], [CreatorUserId], [LastModificationTime], [LastModifierUserId],
                    [IsDeleted], [DeleterUserId], [DeletionTime]
                FROM [Universities];
                
                SET IDENTITY_INSERT [Universities_New] OFF;
            ");

            // ========================================
            // STEP 4: Update Related Tables to use bigint
            // ========================================
            migrationBuilder.Sql(@"
                ALTER TABLE [UniversityFAQs] ALTER COLUMN [UniversityId] bigint NOT NULL;
                ALTER TABLE [UniversityPrograms] ALTER COLUMN [UniversityId] bigint NOT NULL;
            ");

            // ========================================
            // STEP 5: Drop Old Table & Rename New Table
            // ========================================
            migrationBuilder.Sql(@"
                DROP TABLE [Universities];
                EXEC sp_rename 'Universities_New', 'Universities';
            ");

            // ========================================
            // STEP 6: Create Indexes
            // ========================================
            migrationBuilder.CreateIndex(
                name: "IX_Universities_CityId",
                table: "Universities",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Universities_CountryId",
                table: "Universities",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Universities_TenantId",
                table: "Universities",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Universities_Slug",
                table: "Universities",
                column: "Slug",
                unique: true,
                filter: "[Slug] IS NOT NULL AND [IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Universities_SlugAr",
                table: "Universities",
                column: "SlugAr",
                unique: true,
                filter: "[SlugAr] IS NOT NULL AND [IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Universities_Rating",
                table: "Universities",
                column: "Rating");

            migrationBuilder.CreateIndex(
                name: "IX_Universities_IsActive",
                table: "Universities",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Universities_IsFeatured",
                table: "Universities",
                column: "IsFeatured");

            // ========================================
            // STEP 7: Create Foreign Keys
            // ========================================
            migrationBuilder.AddForeignKey(
                name: "FK_Universities_Cities_CityId",
                table: "Universities",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Universities_Countries_CountryId",
                table: "Universities",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            // Re-create FK for related tables
            migrationBuilder.AddForeignKey(
                name: "FK_UniversityFAQs_Universities_UniversityId",
                table: "UniversityFAQs",
                column: "UniversityId",
                principalTable: "Universities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UniversityPrograms_Universities_UniversityId",
                table: "UniversityPrograms",
                column: "UniversityId",
                principalTable: "Universities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            // Re-create indexes for related tables
            migrationBuilder.CreateIndex(
                name: "IX_UniversityFAQs_UniversityId",
                table: "UniversityFAQs",
                column: "UniversityId");

            migrationBuilder.CreateIndex(
                name: "IX_UniversityPrograms_UniversityId",
                table: "UniversityPrograms",
                column: "UniversityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // NOTE: Down migration is complex for table rebuild, reverting requires careful consideration
            // For now, keeping EF scaffold down logic
            migrationBuilder.DropForeignKey(
                name: "FK_Universities_Cities_CityId",
                table: "Universities");

            migrationBuilder.DropForeignKey(
                name: "FK_Universities_Countries_CountryId",
                table: "Universities");

            migrationBuilder.DropIndex(
                name: "IX_Universities_CityId",
                table: "Universities");

            migrationBuilder.DropIndex(
                name: "IX_Universities_CountryId",
                table: "Universities");

            migrationBuilder.DropIndex(
                name: "IX_Universities_TenantId",
                table: "Universities");

            migrationBuilder.DropColumn(
                name: "AboutText",
                table: "Universities");

            migrationBuilder.DropColumn(
                name: "AboutTextAr",
                table: "Universities");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Universities");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Universities");

            migrationBuilder.DropColumn(
                name: "CoverImageBlobId",
                table: "Universities");

            migrationBuilder.DropColumn(
                name: "IntakeMonths",
                table: "Universities");

            migrationBuilder.DropColumn(
                name: "LogoBlobId",
                table: "Universities");

            migrationBuilder.DropColumn(
                name: "MetaDescription",
                table: "Universities");

            migrationBuilder.DropColumn(
                name: "MetaDescriptionAr",
                table: "Universities");

            migrationBuilder.DropColumn(
                name: "OfferLetterFee",
                table: "Universities");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Universities");

            migrationBuilder.AlterColumn<string>(
                name: "SlugAr",
                table: "Universities",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(400)",
                oldMaxLength: 400,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Universities",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(400)",
                oldMaxLength: 400,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NameAr",
                table: "Universities",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Universities",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Universities",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Universities",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Universities",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Universities_City",
                table: "Universities",
                column: "City");

            migrationBuilder.CreateIndex(
                name: "IX_Universities_Country",
                table: "Universities",
                column: "Country");
        }
    }
}

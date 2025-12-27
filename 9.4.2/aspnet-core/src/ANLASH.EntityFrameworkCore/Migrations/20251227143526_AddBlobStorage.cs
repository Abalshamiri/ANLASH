using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ANLASH.Migrations
{
    /// <inheritdoc />
    public partial class AddBlobStorage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppBinaryObjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    Bytes = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EntityType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EntityId = table.Column<long>(type: "bigint", nullable: true),
                    Width = table.Column<int>(type: "int", nullable: true),
                    Height = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_AppBinaryObjects", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppBinaryObjects_Category",
                table: "AppBinaryObjects",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_AppBinaryObjects_CreationTime",
                table: "AppBinaryObjects",
                column: "CreationTime");

            migrationBuilder.CreateIndex(
                name: "IX_AppBinaryObjects_Entity",
                table: "AppBinaryObjects",
                columns: new[] { "EntityType", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppBinaryObjects_TenantId",
                table: "AppBinaryObjects",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppBinaryObjects");
        }
    }
}

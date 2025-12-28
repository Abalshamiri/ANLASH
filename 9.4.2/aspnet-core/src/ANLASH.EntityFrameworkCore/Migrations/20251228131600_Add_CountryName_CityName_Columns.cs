using Microsoft.EntityFrameworkCore.Migrations;

namespace ANLASH.Migrations
{
    public partial class Add_CountryName_CityName_Columns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CountryName",
                table: "Universities",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CityName",
                table: "Universities",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountryName",
                table: "Universities");

            migrationBuilder.DropColumn(
                name: "CityName",
                table: "Universities");
        }
    }
}

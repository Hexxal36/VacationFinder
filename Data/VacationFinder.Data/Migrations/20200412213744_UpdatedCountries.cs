using Microsoft.EntityFrameworkCore.Migrations;

namespace VacationFinder.Data.Migrations
{
    public partial class UpdatedCountries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Countries");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Countries",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Countries");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Countries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

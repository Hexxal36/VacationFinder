namespace VacationFinder.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

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
                defaultValue: string.Empty);
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
                defaultValue: string.Empty);
        }
    }
}

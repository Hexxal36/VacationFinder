using Microsoft.EntityFrameworkCore.Migrations;

namespace VacationFinder.Data.Migrations
{
    public partial class AddedPlacesToOffersAndOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Places",
                table: "Orders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Places",
                table: "Offers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Places",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Places",
                table: "Offers");
        }
    }
}

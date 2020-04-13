namespace VacationFinder.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class CreatedOffers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    Days = table.Column<int>(nullable: false),
                    Nights = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsSpecial = table.Column<bool>(nullable: false),
                    Active_From = table.Column<DateTime>(nullable: false),
                    Active_Until = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    HotelId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false),
                    TransportId = table.Column<int>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offers_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Offers_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Offers_Transports_TransportId",
                        column: x => x.TransportId,
                        principalTable: "Transports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OfferImg",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: false),
                    OfferId = table.Column<int>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferImg", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfferImg_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OfferImg_IsDeleted",
                table: "OfferImg",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_OfferImg_OfferId",
                table: "OfferImg",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_HotelId",
                table: "Offers",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_IsDeleted",
                table: "Offers",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_TagId",
                table: "Offers",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_TransportId",
                table: "Offers",
                column: "TransportId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfferImg");

            migrationBuilder.DropTable(
                name: "Offers");
        }
    }
}

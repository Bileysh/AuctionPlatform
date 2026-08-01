using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAuctionIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AuctionItems_Status_CategoryId",
                table: "AuctionItems",
                columns: new[] { "Status", "CategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_AuctionItems_Status_EndsAt",
                table: "AuctionItems",
                columns: new[] { "Status", "EndsAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AuctionItems_Status_CategoryId",
                table: "AuctionItems");

            migrationBuilder.DropIndex(
                name: "IX_AuctionItems_Status_EndsAt",
                table: "AuctionItems");
        }
    }
}

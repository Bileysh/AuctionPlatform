using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Transactions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Bids",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AuctionItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Bids");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AuctionItems");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cryptotracker.Shared.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreatew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PriceHistory_Timestamp",
                table: "PriceHistory");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PriceHistory_Timestamp",
                table: "PriceHistory",
                column: "Timestamp");
        }
    }
}

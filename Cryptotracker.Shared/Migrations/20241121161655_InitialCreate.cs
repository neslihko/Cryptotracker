using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cryptotracker.Shared.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ChangePercent24Hr",
                table: "Cryptocurrencies",
                type: "numeric(40,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(10,2)");

            migrationBuilder.AddColumn<decimal>(
                name: "MaxSupply",
                table: "Cryptocurrencies",
                type: "numeric(40,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Supply",
                table: "Cryptocurrencies",
                type: "numeric(40,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "VWAP24Hr",
                table: "Cryptocurrencies",
                type: "numeric(40,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxSupply",
                table: "Cryptocurrencies");

            migrationBuilder.DropColumn(
                name: "Supply",
                table: "Cryptocurrencies");

            migrationBuilder.DropColumn(
                name: "VWAP24Hr",
                table: "Cryptocurrencies");

            migrationBuilder.AlterColumn<decimal>(
                name: "ChangePercent24Hr",
                table: "Cryptocurrencies",
                type: "numeric(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(40,8)",
                oldPrecision: 18,
                oldScale: 8);
        }
    }
}

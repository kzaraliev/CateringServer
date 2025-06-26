using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catering.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDeliveryFee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DeliveryFee",
                table: "Restaurants",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                comment: "Delivery fee of the restaurant");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryFee",
                table: "Restaurants");
        }
    }
}

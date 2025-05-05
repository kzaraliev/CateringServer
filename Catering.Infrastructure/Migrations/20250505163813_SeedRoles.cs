using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Catering.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "32dbb61d-0b46-4c77-8449-f3a633b6a72b", "459d0654-cabb-4c9d-bc6d-b81d8d72cb51", "RestaurantOwner", "RESTAURANTOWNER" },
                    { "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e", "cd76a1be-26ad-4f32-b29d-f0c0d5f7cd37", "SuperAdmin", "SUPERADMIN" },
                    { "9054b99c-81ba-465d-bb62-606df48b58b9", "76a5f411-9a1a-4318-b2d0-c826fc86b32d", "SiteAdmin", "SITEADMIN" },
                    { "b9711b31-d6cf-4c89-b7c0-9634db87154d", "8d103fbd-2ede-4e27-ade3-3d0c959935b1", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "32dbb61d-0b46-4c77-8449-f3a633b6a72b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9054b99c-81ba-465d-bb62-606df48b58b9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b9711b31-d6cf-4c89-b7c0-9634db87154d");
        }
    }
}

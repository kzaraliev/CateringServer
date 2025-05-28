using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catering.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRoleNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Admin", "ADMIN" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9054b99c-81ba-465d-bb62-606df48b58b9",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Moderator", "MODERATOR" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "SuperAdmin", "SUPERADMIN" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9054b99c-81ba-465d-bb62-606df48b58b9",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "SiteAdmin", "SITEADMIN" });
        }
    }
}

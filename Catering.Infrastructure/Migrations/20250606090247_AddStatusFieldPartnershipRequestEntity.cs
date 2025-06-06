using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catering.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusFieldPartnershipRequestEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "PartnershipRequests");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "PartnershipRequests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "The status of the partner request.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "PartnershipRequests");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "PartnershipRequests",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether the request has been approved");
        }
    }
}

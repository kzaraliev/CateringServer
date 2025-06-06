using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catering.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFieldsPartnershipRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvitationToken",
                table: "PartnershipRequests");

            migrationBuilder.DropColumn(
                name: "TokenExpiresAt",
                table: "PartnershipRequests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InvitationToken",
                table: "PartnershipRequests",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true,
                comment: "Invitation token for unregistered users");

            migrationBuilder.AddColumn<DateTime>(
                name: "TokenExpiresAt",
                table: "PartnershipRequests",
                type: "datetime2",
                nullable: true,
                comment: "Expiration timestamp for the invitation token");
        }
    }
}

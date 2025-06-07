using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catering.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangePropNamePartnershipRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "PartnershipRequests");

            migrationBuilder.AddColumn<DateTime>(
                name: "ProcessedAt",
                table: "PartnershipRequests",
                type: "datetime2",
                nullable: true,
                comment: "Timestamp of request processing");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProcessedAt",
                table: "PartnershipRequests");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                table: "PartnershipRequests",
                type: "datetime2",
                nullable: true,
                comment: "Timestamp of approval");
        }
    }
}

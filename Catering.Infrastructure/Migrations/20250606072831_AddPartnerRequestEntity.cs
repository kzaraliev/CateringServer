using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catering.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPartnerRequestEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PartnershipRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Partnership Request Identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Name of the restaurant in the request"),
                    ContactEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Email of the person requesting the partnership"),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "Optional message from the requester"),
                    RequestedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true, comment: "User ID of the requester (if logged in)"),
                    InvitationToken = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true, comment: "Invitation token for unregistered users"),
                    TokenExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Expiration timestamp for the invitation token"),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false, comment: "Whether the request has been approved"),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Timestamp of approval"),
                    RestaurantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartnershipRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartnershipRequests_AspNetUsers_RequestedByUserId",
                        column: x => x.RequestedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PartnershipRequests_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id");
                },
                comment: "Represents a request for a restaurant partnership in the platform.");

            migrationBuilder.CreateIndex(
                name: "IX_PartnershipRequests_RequestedByUserId",
                table: "PartnershipRequests",
                column: "RequestedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PartnershipRequests_RestaurantId",
                table: "PartnershipRequests",
                column: "RestaurantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PartnershipRequests");
        }
    }
}

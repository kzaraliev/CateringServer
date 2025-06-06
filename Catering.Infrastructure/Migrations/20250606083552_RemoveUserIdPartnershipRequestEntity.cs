using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catering.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserIdPartnershipRequestEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartnershipRequests_AspNetUsers_RequestedByUserId",
                table: "PartnershipRequests");

            migrationBuilder.DropIndex(
                name: "IX_PartnershipRequests_RequestedByUserId",
                table: "PartnershipRequests");

            migrationBuilder.DropColumn(
                name: "RequestedByUserId",
                table: "PartnershipRequests");

            migrationBuilder.AlterColumn<int>(
                name: "RestaurantId",
                table: "PartnershipRequests",
                type: "int",
                nullable: true,
                comment: "Foreign key to the related restaurant created from this request",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "RestaurantId",
                table: "PartnershipRequests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Foreign key to the related restaurant created from this request");

            migrationBuilder.AddColumn<string>(
                name: "RequestedByUserId",
                table: "PartnershipRequests",
                type: "nvarchar(450)",
                nullable: true,
                comment: "User ID of the requester (if logged in)");

            migrationBuilder.CreateIndex(
                name: "IX_PartnershipRequests_RequestedByUserId",
                table: "PartnershipRequests",
                column: "RequestedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PartnershipRequests_AspNetUsers_RequestedByUserId",
                table: "PartnershipRequests",
                column: "RequestedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}

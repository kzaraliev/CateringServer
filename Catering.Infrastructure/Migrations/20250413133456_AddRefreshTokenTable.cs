using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catering.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokenTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Refresh token identifier.")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "The token string."),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "The expiration date and time of the token."),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "The date and time when the token was created."),
                    Revoked = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "The date and time when the token was revoked, if any."),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "Identifier of the user the token belongs to.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Represents a refresh token used for renewing JWT tokens.");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");
        }
    }
}

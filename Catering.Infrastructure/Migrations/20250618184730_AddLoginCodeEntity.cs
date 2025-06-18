using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catering.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLoginCodeEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoginCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Unique identifier for the login code.")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false, comment: "The actual numeric or alphanumeric code sent to the user."),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "UTC date and time when the login code was created."),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "UTC date and time when the login code expires."),
                    UsedAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "UTC date and time when the login code was used. Null if not yet used."),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "ID of the user for whom this login code was generated.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoginCodes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Represents a one-time login code (OTP) generated for passwordless authentication.");

            migrationBuilder.CreateIndex(
                name: "IX_LoginCodes_UserId",
                table: "LoginCodes",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoginCodes");
        }
    }
}

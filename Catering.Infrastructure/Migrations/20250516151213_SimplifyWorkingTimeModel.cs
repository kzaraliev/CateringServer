using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catering.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SimplifyWorkingTimeModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_WorkingTimes_WorkingTimeId",
                table: "Restaurants");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkingDays_WorkingTimes_WorkingTimeId",
                table: "WorkingDays");

            migrationBuilder.DropTable(
                name: "WorkingTimes");

            migrationBuilder.DropIndex(
                name: "IX_WorkingDays_WorkingTimeId",
                table: "WorkingDays");

            migrationBuilder.DropIndex(
                name: "IX_Restaurants_WorkingTimeId",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "WorkingTimeId",
                table: "WorkingDays");

            migrationBuilder.DropColumn(
                name: "WorkingTimeId",
                table: "Restaurants");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "OpenTime",
                table: "WorkingDays",
                type: "time",
                nullable: true,
                comment: "Opening time for the day.",
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldComment: "Opening time for the day.");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "CloseTime",
                table: "WorkingDays",
                type: "time",
                nullable: true,
                comment: "Closing time for the day.",
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldComment: "Closing time for the day.");

            migrationBuilder.AddColumn<int>(
                name: "RestaurantId",
                table: "WorkingDays",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Foreign key to the related restaurant.");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingDays_RestaurantId",
                table: "WorkingDays",
                column: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingDays_Restaurants_RestaurantId",
                table: "WorkingDays",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkingDays_Restaurants_RestaurantId",
                table: "WorkingDays");

            migrationBuilder.DropIndex(
                name: "IX_WorkingDays_RestaurantId",
                table: "WorkingDays");

            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "WorkingDays");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "OpenTime",
                table: "WorkingDays",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0),
                comment: "Opening time for the day.",
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldNullable: true,
                oldComment: "Opening time for the day.");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "CloseTime",
                table: "WorkingDays",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0),
                comment: "Closing time for the day.",
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldNullable: true,
                oldComment: "Closing time for the day.");

            migrationBuilder.AddColumn<int>(
                name: "WorkingTimeId",
                table: "WorkingDays",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Foreign key to the related WorkingTime schedule.");

            migrationBuilder.AddColumn<int>(
                name: "WorkingTimeId",
                table: "Restaurants",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Working Time Schedule Identifier");

            migrationBuilder.CreateTable(
                name: "WorkingTimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Working Time Identifier")
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingTimes", x => x.Id);
                },
                comment: "Represents the overall working time schedule for a restaurant.");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingDays_WorkingTimeId",
                table: "WorkingDays",
                column: "WorkingTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_WorkingTimeId",
                table: "Restaurants",
                column: "WorkingTimeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_WorkingTimes_WorkingTimeId",
                table: "Restaurants",
                column: "WorkingTimeId",
                principalTable: "WorkingTimes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingDays_WorkingTimes_WorkingTimeId",
                table: "WorkingDays",
                column: "WorkingTimeId",
                principalTable: "WorkingTimes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

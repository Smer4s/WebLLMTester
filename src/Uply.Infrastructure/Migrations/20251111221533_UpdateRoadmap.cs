using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uply.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoadmap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "RoadmapTask");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "RoadmapTask");

            migrationBuilder.AddColumn<int>(
                name: "TaskNumber",
                table: "RoadmapTask",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "TaskReportId",
                table: "RoadmapTask",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TaskReport",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    PhotoUrls = table.Column<string[]>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskReport", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoadmapTask_TaskReportId",
                table: "RoadmapTask",
                column: "TaskReportId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RoadmapTask_TaskReport_TaskReportId",
                table: "RoadmapTask",
                column: "TaskReportId",
                principalTable: "TaskReport",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoadmapTask_TaskReport_TaskReportId",
                table: "RoadmapTask");

            migrationBuilder.DropTable(
                name: "TaskReport");

            migrationBuilder.DropIndex(
                name: "IX_RoadmapTask_TaskReportId",
                table: "RoadmapTask");

            migrationBuilder.DropColumn(
                name: "TaskNumber",
                table: "RoadmapTask");

            migrationBuilder.DropColumn(
                name: "TaskReportId",
                table: "RoadmapTask");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "RoadmapTask",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "RoadmapTask",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}

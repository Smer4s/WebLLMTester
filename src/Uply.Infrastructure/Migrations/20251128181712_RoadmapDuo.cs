using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uply.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RoadmapDuo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDuo",
                table: "Roadmap",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDuo",
                table: "Roadmap");
        }
    }
}

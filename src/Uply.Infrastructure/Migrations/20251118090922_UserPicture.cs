using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uply.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserPicture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "User",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "User");
        }
    }
}

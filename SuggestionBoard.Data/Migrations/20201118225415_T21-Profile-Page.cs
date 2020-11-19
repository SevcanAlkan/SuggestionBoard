using Microsoft.EntityFrameworkCore.Migrations;

namespace SuggestionBoard.Data.Migrations
{
    public partial class T21ProfilePage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PictureUrl",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureUrl",
                table: "Users");
        }
    }
}

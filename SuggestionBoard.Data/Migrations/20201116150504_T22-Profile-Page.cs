using Microsoft.EntityFrameworkCore.Migrations;

namespace SuggestionBoard.Data.Migrations
{
    public partial class T22ProfilePage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReactionAmount",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SuggestionAmount",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReactionAmount",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SuggestionAmount",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

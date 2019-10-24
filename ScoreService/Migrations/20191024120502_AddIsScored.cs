using Microsoft.EntityFrameworkCore.Migrations;

namespace ScoreService.Migrations
{
    public partial class AddIsScored : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsScored",
                table: "TeamEntity",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsScored",
                table: "TeamEntity");
        }
    }
}

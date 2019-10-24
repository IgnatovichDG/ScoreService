using Microsoft.EntityFrameworkCore.Migrations;

namespace ScoreService.Migrations
{
    public partial class ChangeScoreType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Score",
                table: "ScoreEntity",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Score",
                table: "ScoreEntity",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}

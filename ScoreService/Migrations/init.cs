using Microsoft.EntityFrameworkCore.Migrations;

namespace ScoreService.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScoreCategoryEntity",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Alias = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Kind = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreCategoryEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserEntity",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Login = table.Column<string>(nullable: true),
                    PasswordSalt = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeamEntity",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamEntity_UserEntity_UserId",
                        column: x => x.UserId,
                        principalTable: "UserEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScoreEntity",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<long>(nullable: false),
                    TeamId = table.Column<long>(nullable: false),
                    Score = table.Column<int>(nullable: false),
                    CategoryId = table.Column<long>(nullable: true),
                    ScoreCategoryEntityId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScoreEntity_ScoreCategoryEntity_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ScoreCategoryEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScoreEntity_ScoreCategoryEntity_ScoreCategoryEntityId",
                        column: x => x.ScoreCategoryEntityId,
                        principalTable: "ScoreCategoryEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreEntity_TeamEntity_TeamId",
                        column: x => x.TeamId,
                        principalTable: "TeamEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreEntity_UserEntity_UserId",
                        column: x => x.UserId,
                        principalTable: "UserEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScoreEntity_CategoryId",
                table: "ScoreEntity",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreEntity_ScoreCategoryEntityId",
                table: "ScoreEntity",
                column: "ScoreCategoryEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreEntity_TeamId",
                table: "ScoreEntity",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreEntity_UserId",
                table: "ScoreEntity",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamEntity_UserId",
                table: "TeamEntity",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScoreEntity");

            migrationBuilder.DropTable(
                name: "ScoreCategoryEntity");

            migrationBuilder.DropTable(
                name: "TeamEntity");

            migrationBuilder.DropTable(
                name: "UserEntity");
        }
    }
}

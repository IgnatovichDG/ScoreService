using Microsoft.EntityFrameworkCore.Migrations;

namespace ScoreService.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BindSettingsEntity",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MaxTeamForUserCounter = table.Column<int>(nullable: false),
                    MinUsersForTeamCounter = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BindSettingsEntity", x => x.Id);
                });

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
                name: "ZoneEntity",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZoneEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeamEntity",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    ZoneId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamEntity_ZoneEntity_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "ZoneEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserEntity",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Login = table.Column<string>(nullable: true),
                    PasswordSalt = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    ZoneId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserEntity_ZoneEntity_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "ZoneEntity",
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
                    Score = table.Column<string>(nullable: true),
                    CategoryId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScoreEntity_ScoreCategoryEntity_CategoryId",
                        column: x => x.CategoryId,
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

            migrationBuilder.CreateTable(
                name: "UserTeamRelation",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false),
                    TeamId = table.Column<long>(nullable: false),
                    IsScored = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTeamRelation", x => new { x.UserId, x.TeamId });
                    table.ForeignKey(
                        name: "FK_UserTeamRelation_TeamEntity_TeamId",
                        column: x => x.TeamId,
                        principalTable: "TeamEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTeamRelation_UserEntity_UserId",
                        column: x => x.UserId,
                        principalTable: "UserEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "BindSettingsEntity",
                columns: new[] { "Id", "MaxTeamForUserCounter", "MinUsersForTeamCounter" },
                values: new object[] { 1L, 15, 3 });

            migrationBuilder.InsertData(
                table: "ZoneEntity",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1L, "A" });

            migrationBuilder.InsertData(
                table: "ZoneEntity",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2L, "B" });

            migrationBuilder.CreateIndex(
                name: "IX_ScoreEntity_CategoryId",
                table: "ScoreEntity",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreEntity_TeamId",
                table: "ScoreEntity",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreEntity_UserId",
                table: "ScoreEntity",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamEntity_ZoneId",
                table: "TeamEntity",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_UserEntity_ZoneId",
                table: "UserEntity",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTeamRelation_TeamId",
                table: "UserTeamRelation",
                column: "TeamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BindSettingsEntity");

            migrationBuilder.DropTable(
                name: "ScoreEntity");

            migrationBuilder.DropTable(
                name: "UserTeamRelation");

            migrationBuilder.DropTable(
                name: "ScoreCategoryEntity");

            migrationBuilder.DropTable(
                name: "TeamEntity");

            migrationBuilder.DropTable(
                name: "UserEntity");

            migrationBuilder.DropTable(
                name: "ZoneEntity");
        }
    }
}

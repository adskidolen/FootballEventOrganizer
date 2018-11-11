using Microsoft.EntityFrameworkCore.Migrations;

namespace Footeo.Data.Migrations
{
    public partial class Test_TeamLeagues_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Leagues_LeagueId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_LeagueId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Drawn",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "GoalDifference",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "GoalsAgainst",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "GoalsFor",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "LeagueId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Lost",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "PlayedMatches",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Points",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Won",
                table: "Teams");

            migrationBuilder.CreateTable(
                name: "TeamsLeagues",
                columns: table => new
                {
                    TeamId = table.Column<int>(nullable: false),
                    LeagueId = table.Column<int>(nullable: false),
                    Points = table.Column<int>(nullable: false),
                    Position = table.Column<int>(nullable: false),
                    GoalsFor = table.Column<int>(nullable: false),
                    GoalsAgainst = table.Column<int>(nullable: false),
                    GoalDifference = table.Column<int>(nullable: false),
                    PlayedMatches = table.Column<int>(nullable: false),
                    Won = table.Column<int>(nullable: false),
                    Drawn = table.Column<int>(nullable: false),
                    Lost = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamsLeagues", x => new { x.TeamId, x.LeagueId });
                    table.ForeignKey(
                        name: "FK_TeamsLeagues_Leagues_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "Leagues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamsLeagues_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeamsLeagues_LeagueId",
                table: "TeamsLeagues",
                column: "LeagueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamsLeagues");

            migrationBuilder.AddColumn<int>(
                name: "Drawn",
                table: "Teams",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GoalDifference",
                table: "Teams",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GoalsAgainst",
                table: "Teams",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GoalsFor",
                table: "Teams",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LeagueId",
                table: "Teams",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Lost",
                table: "Teams",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlayedMatches",
                table: "Teams",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "Teams",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "Teams",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Won",
                table: "Teams",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_LeagueId",
                table: "Teams",
                column: "LeagueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Leagues_LeagueId",
                table: "Teams",
                column: "LeagueId",
                principalTable: "Leagues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

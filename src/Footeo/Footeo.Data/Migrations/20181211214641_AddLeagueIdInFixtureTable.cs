using Microsoft.EntityFrameworkCore.Migrations;

namespace Footeo.Data.Migrations
{
    public partial class AddLeagueIdInFixtureTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fixtures_Leagues_LeagueId",
                table: "Fixtures");

            migrationBuilder.AlterColumn<int>(
                name: "LeagueId",
                table: "Fixtures",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Fixtures_Leagues_LeagueId",
                table: "Fixtures",
                column: "LeagueId",
                principalTable: "Leagues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fixtures_Leagues_LeagueId",
                table: "Fixtures");

            migrationBuilder.AlterColumn<int>(
                name: "LeagueId",
                table: "Fixtures",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Fixtures_Leagues_LeagueId",
                table: "Fixtures",
                column: "LeagueId",
                principalTable: "Leagues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

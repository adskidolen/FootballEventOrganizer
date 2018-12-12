using Microsoft.EntityFrameworkCore.Migrations;

namespace Footeo.Data.Migrations
{
    public partial class MatchEntityModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "HomeTeamGoals",
                table: "Matches",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "AwayTeamGoals",
                table: "Matches",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "HomeTeamGoals",
                table: "Matches",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AwayTeamGoals",
                table: "Matches",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}

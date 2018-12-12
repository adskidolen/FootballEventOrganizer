using Microsoft.EntityFrameworkCore.Migrations;

namespace Footeo.Data.Migrations
{
    public partial class AddedFullNameForPlayersAndReferees : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Referees",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Nickname",
                table: "Players",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Players",
                maxLength: 30,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Referees");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Players");

            migrationBuilder.AlterColumn<string>(
                name: "Nickname",
                table: "Players",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}

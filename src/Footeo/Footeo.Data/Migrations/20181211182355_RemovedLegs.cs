using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Footeo.Data.Migrations
{
    public partial class RemovedLegs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Legs_LegId",
                table: "Matches");

            migrationBuilder.DropTable(
                name: "Legs");

            migrationBuilder.RenameColumn(
                name: "LegId",
                table: "Matches",
                newName: "FixtureId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_LegId",
                table: "Matches",
                newName: "IX_Matches_FixtureId");

            migrationBuilder.AlterColumn<string>(
                name: "Result",
                table: "Matches",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 7);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Fixtures_FixtureId",
                table: "Matches",
                column: "FixtureId",
                principalTable: "Fixtures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Fixtures_FixtureId",
                table: "Matches");

            migrationBuilder.RenameColumn(
                name: "FixtureId",
                table: "Matches",
                newName: "LegId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_FixtureId",
                table: "Matches",
                newName: "IX_Matches_LegId");

            migrationBuilder.AlterColumn<string>(
                name: "Result",
                table: "Matches",
                maxLength: 7,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Legs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FixtureId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Legs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Legs_Fixtures_FixtureId",
                        column: x => x.FixtureId,
                        principalTable: "Fixtures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Legs_FixtureId",
                table: "Legs",
                column: "FixtureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Legs_LegId",
                table: "Matches",
                column: "LegId",
                principalTable: "Legs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

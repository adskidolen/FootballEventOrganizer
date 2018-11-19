using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Footeo.Data.Migrations
{
    public partial class Changes_In_Leagues_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "News",
                table: "Leagues");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Leagues",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Leagues",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Leagues");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Leagues");

            migrationBuilder.AddColumn<string>(
                name: "News",
                table: "Leagues",
                nullable: false,
                defaultValue: "");
        }
    }
}

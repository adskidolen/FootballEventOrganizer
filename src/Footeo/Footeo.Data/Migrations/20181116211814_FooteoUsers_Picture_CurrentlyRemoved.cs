using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Footeo.Data.Migrations
{
    public partial class FooteoUsers_Picture_CurrentlyRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Picture",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "Role",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Role",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Picture",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new byte[] {  });
        }
    }
}

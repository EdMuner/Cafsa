using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cafsa.Web.Migrations
{
    public partial class modofyService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Services");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Services",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Services");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Services",
                nullable: false,
                defaultValue: false);
        }
    }
}

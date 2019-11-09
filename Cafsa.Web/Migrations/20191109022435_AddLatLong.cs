using Microsoft.EntityFrameworkCore.Migrations;

namespace Cafsa.Web.Migrations
{
    public partial class AddLatLong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Activities",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Activities",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Activities");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "AspNetUsers",
                maxLength: 100,
                nullable: true);
        }
    }
}

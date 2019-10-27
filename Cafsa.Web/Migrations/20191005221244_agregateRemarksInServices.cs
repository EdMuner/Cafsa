using Microsoft.EntityFrameworkCore.Migrations;

namespace Cafsa.Web.Migrations
{
    public partial class agregateRemarksInServices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "Services",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "Services");
        }
    }
}

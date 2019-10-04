using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cafsa.Web.Migrations
{
    public partial class changueDatabaseForProblem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Referees_RefereeTypes_RefereeTypeId",
                table: "Referees");

            migrationBuilder.DropTable(
                name: "RefereeTypes");

            migrationBuilder.DropIndex(
                name: "IX_Referees_RefereeTypeId",
                table: "Referees");

            migrationBuilder.DropColumn(
                name: "RefereeTypeId",
                table: "Referees");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Referees",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Referees");

            migrationBuilder.AddColumn<int>(
                name: "RefereeTypeId",
                table: "Referees",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RefereeTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefereeTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Referees_RefereeTypeId",
                table: "Referees",
                column: "RefereeTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Referees_RefereeTypes_RefereeTypeId",
                table: "Referees",
                column: "RefereeTypeId",
                principalTable: "RefereeTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

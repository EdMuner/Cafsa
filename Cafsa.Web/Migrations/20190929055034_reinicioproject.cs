using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cafsa.Web.Migrations
{
    public partial class reinicioproject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefereeImages");

            migrationBuilder.DropColumn(
                name: "Neighborhood",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Contracts");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Services",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Neighborhood",
                table: "Services",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RefereeId",
                table: "Services",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Contracts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "AddUserViewModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Document = table.Column<string>(maxLength: 20, nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    Address = table.Column<string>(maxLength: 100, nullable: true),
                    Phone = table.Column<string>(maxLength: 50, nullable: true),
                    Username = table.Column<string>(maxLength: 100, nullable: false),
                    Category = table.Column<string>(maxLength: 50, nullable: false),
                    Password = table.Column<string>(maxLength: 20, nullable: false),
                    PasswordConfirm = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddUserViewModel", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Services_RefereeId",
                table: "Services",
                column: "RefereeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Referees_RefereeId",
                table: "Services",
                column: "RefereeId",
                principalTable: "Referees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Referees_RefereeId",
                table: "Services");

            migrationBuilder.DropTable(
                name: "AddUserViewModel");

            migrationBuilder.DropIndex(
                name: "IX_Services_RefereeId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Neighborhood",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "RefereeId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Contracts");

            migrationBuilder.AddColumn<string>(
                name: "Neighborhood",
                table: "Contracts",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Quantity",
                table: "Contracts",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "RefereeImages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ImageUrl = table.Column<string>(nullable: true),
                    RefereeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefereeImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefereeImages_Referees_RefereeId",
                        column: x => x.RefereeId,
                        principalTable: "Referees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefereeImages_RefereeId",
                table: "RefereeImages",
                column: "RefereeId");
        }
    }
}

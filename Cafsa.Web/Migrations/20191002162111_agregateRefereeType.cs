using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cafsa.Web.Migrations
{
    public partial class agregateRefereeType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "CategoryTypes");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "CategoryTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoryTypeId = table.Column<int>(nullable: true),
                    RefereeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_CategoryTypes_CategoryTypeId",
                        column: x => x.CategoryTypeId,
                        principalTable: "CategoryTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Categories_Referees_RefereeId",
                        column: x => x.RefereeId,
                        principalTable: "Referees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CategoryTypeId",
                table: "Categories",
                column: "CategoryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_RefereeId",
                table: "Categories",
                column: "RefereeId");
        }
    }
}

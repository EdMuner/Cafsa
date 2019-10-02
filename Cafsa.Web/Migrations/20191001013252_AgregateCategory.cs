using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cafsa.Web.Migrations
{
    public partial class AgregateCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Services_ServiceId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceImages_Services_ServiceId",
                table: "ServiceImages");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Referees_RefereeId",
                table: "Services");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_ServiceTypes_ServiceTypeId",
                table: "Services");

            migrationBuilder.DropTable(
                name: "AddUserViewModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Services",
                table: "Services");

            migrationBuilder.RenameTable(
                name: "Services",
                newName: "Service");

            migrationBuilder.RenameIndex(
                name: "IX_Services_ServiceTypeId",
                table: "Service",
                newName: "IX_Service_ServiceTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Services_RefereeId",
                table: "Service",
                newName: "IX_Service_RefereeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Service",
                table: "Service",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Service_ServiceId",
                table: "Contracts",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Service_Referees_RefereeId",
                table: "Service",
                column: "RefereeId",
                principalTable: "Referees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Service_ServiceTypes_ServiceTypeId",
                table: "Service",
                column: "ServiceTypeId",
                principalTable: "ServiceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceImages_Service_ServiceId",
                table: "ServiceImages",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Service_ServiceId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Service_Referees_RefereeId",
                table: "Service");

            migrationBuilder.DropForeignKey(
                name: "FK_Service_ServiceTypes_ServiceTypeId",
                table: "Service");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceImages_Service_ServiceId",
                table: "ServiceImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Service",
                table: "Service");

            migrationBuilder.RenameTable(
                name: "Service",
                newName: "Services");

            migrationBuilder.RenameIndex(
                name: "IX_Service_ServiceTypeId",
                table: "Services",
                newName: "IX_Services_ServiceTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Service_RefereeId",
                table: "Services",
                newName: "IX_Services_RefereeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Services",
                table: "Services",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AddUserViewModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(maxLength: 100, nullable: true),
                    Category = table.Column<string>(maxLength: 50, nullable: false),
                    Document = table.Column<string>(maxLength: 20, nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    Password = table.Column<string>(maxLength: 20, nullable: false),
                    PasswordConfirm = table.Column<string>(maxLength: 20, nullable: false),
                    Phone = table.Column<string>(maxLength: 50, nullable: true),
                    Username = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddUserViewModel", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Services_ServiceId",
                table: "Contracts",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceImages_Services_ServiceId",
                table: "ServiceImages",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Referees_RefereeId",
                table: "Services",
                column: "RefereeId",
                principalTable: "Referees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_ServiceTypes_ServiceTypeId",
                table: "Services",
                column: "ServiceTypeId",
                principalTable: "ServiceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

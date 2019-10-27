using Microsoft.EntityFrameworkCore.Migrations;

namespace Cafsa.Web.Migrations
{
    public partial class refactorDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Referees_RefereeId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_RefereeId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "RefereeId",
                table: "Contracts");

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Services",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Clients",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RefereeId",
                table: "Clients",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_ClientId",
                table: "Services",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_RefereeId",
                table: "Clients",
                column: "RefereeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Referees_RefereeId",
                table: "Clients",
                column: "RefereeId",
                principalTable: "Referees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Clients_ClientId",
                table: "Services",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Referees_RefereeId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Clients_ClientId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_ClientId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Clients_RefereeId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "RefereeId",
                table: "Clients");

            migrationBuilder.AddColumn<int>(
                name: "RefereeId",
                table: "Contracts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_RefereeId",
                table: "Contracts",
                column: "RefereeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Referees_RefereeId",
                table: "Contracts",
                column: "RefereeId",
                principalTable: "Referees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

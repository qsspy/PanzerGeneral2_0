using Microsoft.EntityFrameworkCore.Migrations;

namespace PanzerGeneral2_0.Migrations
{
    public partial class Version_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "canMove",
                table: "Unit",
                newName: "CanMove");

            migrationBuilder.RenameColumn(
                name: "canAttack",
                table: "Unit",
                newName: "CanAttack");

            migrationBuilder.AddColumn<int>(
                name: "RoundNumber",
                table: "GameState",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoundNumber",
                table: "GameState");

            migrationBuilder.RenameColumn(
                name: "CanMove",
                table: "Unit",
                newName: "canMove");

            migrationBuilder.RenameColumn(
                name: "CanAttack",
                table: "Unit",
                newName: "canAttack");
        }
    }
}

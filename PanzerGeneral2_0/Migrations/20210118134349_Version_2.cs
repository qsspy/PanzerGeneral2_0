using Microsoft.EntityFrameworkCore.Migrations;

namespace PanzerGeneral2_0.Migrations
{
    public partial class Version_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "canAttack",
                table: "Unit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "canMove",
                table: "Unit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "canAttack",
                table: "Unit");

            migrationBuilder.DropColumn(
                name: "canMove",
                table: "Unit");
        }
    }
}

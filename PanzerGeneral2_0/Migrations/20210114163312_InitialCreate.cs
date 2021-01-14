using Microsoft.EntityFrameworkCore.Migrations;

namespace PanzerGeneral2_0.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameState",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CurrentTurn = table.Column<int>(nullable: true),
                    WinnerTeamCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameState", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Unit",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TeamCode = table.Column<int>(nullable: false),
                    UnitKind = table.Column<int>(nullable: false),
                    XPosition = table.Column<int>(nullable: false),
                    YPosition = table.Column<int>(nullable: false),
                    Hp = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unit", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameState");

            migrationBuilder.DropTable(
                name: "Unit");
        }
    }
}

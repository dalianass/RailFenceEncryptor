using Microsoft.EntityFrameworkCore.Migrations;

namespace RailFenceAPI.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Desifrovanja",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecZaDesifrovanje = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dubina = table.Column<int>(type: "int", nullable: false),
                    DesifrovanaRec = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Desifrovanja", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sifrovanja",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecZaSifrovanje = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dubina = table.Column<int>(type: "int", nullable: false),
                    SifrovanaRec = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sifrovanja", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Desifrovanja");

            migrationBuilder.DropTable(
                name: "Sifrovanja");
        }
    }
}

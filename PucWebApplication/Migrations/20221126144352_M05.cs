using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PucWebApplication.Migrations
{
    public partial class M05 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmpFileName",
                table: "Usuarios",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "EmpPhotoPath",
                table: "Usuarios",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmpFileName",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "EmpPhotoPath",
                table: "Usuarios");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmCare.DAL.Migrations
{
    public partial class _5898 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Units");

            migrationBuilder.AddColumn<int>(
                name: "UnitValue",
                table: "Units",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitValue",
                table: "Units");

            migrationBuilder.AddColumn<byte>(
                name: "Status",
                table: "Units",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }
    }
}

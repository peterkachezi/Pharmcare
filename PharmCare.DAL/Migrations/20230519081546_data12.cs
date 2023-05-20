using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmCare.DAL.Migrations
{
    public partial class data12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "Status",
                table: "Stocks",
                type: "tinyint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Stocks");
        }
    }
}

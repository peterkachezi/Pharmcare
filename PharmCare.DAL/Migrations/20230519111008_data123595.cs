using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmCare.DAL.Migrations
{
    public partial class data123595 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "Status",
                table: "GoodsReceivedHistories",
                type: "tinyint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "GoodsReceivedHistories");
        }
    }
}

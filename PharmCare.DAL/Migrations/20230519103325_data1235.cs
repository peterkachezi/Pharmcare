using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmCare.DAL.Migrations
{
    public partial class data1235 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CostPrice",
                table: "GoodsReceivedHistories",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalCostPrice",
                table: "GoodsReceivedHistories",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostPrice",
                table: "GoodsReceivedHistories");

            migrationBuilder.DropColumn(
                name: "TotalCostPrice",
                table: "GoodsReceivedHistories");
        }
    }
}

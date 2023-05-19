using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmCare.DAL.Migrations
{
    public partial class cp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SellingPrice",
                table: "Medicines");

            migrationBuilder.AddColumn<decimal>(
                name: "CostPrice",
                table: "Stocks",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SellingPrice",
                table: "Stocks",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostPrice",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "SellingPrice",
                table: "Stocks");

            migrationBuilder.AddColumn<decimal>(
                name: "SellingPrice",
                table: "Medicines",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}

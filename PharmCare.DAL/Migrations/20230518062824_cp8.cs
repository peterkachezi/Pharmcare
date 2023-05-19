using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmCare.DAL.Migrations
{
    public partial class cp8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ManufacturerPrice",
                table: "Medicines");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ManufacturerPrice",
                table: "Medicines",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}

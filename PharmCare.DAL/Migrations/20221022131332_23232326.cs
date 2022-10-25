using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmCare.DAL.Migrations
{
    public partial class _23232326 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "PaymentStatus",
                table: "Prescriptions",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "BillNo",
                table: "PrescriptionDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte>(
                name: "PaymentStatus",
                table: "PrescriptionDetails",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "BillNo",
                table: "PrescriptionDetails");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "PrescriptionDetails");
        }
    }
}

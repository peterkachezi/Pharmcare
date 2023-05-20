using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmCare.DAL.Migrations
{
    public partial class _55 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvoicePayments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AmountPayable = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Balance = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AmountPaid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentMode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoicePayments", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoicePayments");
        }
    }
}

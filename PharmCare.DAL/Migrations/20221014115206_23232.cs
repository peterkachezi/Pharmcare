using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmCare.DAL.Migrations
{
    public partial class _23232 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrescriptionFor",
                table: "Prescriptions");

            migrationBuilder.AddColumn<Guid>(
                name: "TreatmentForId",
                table: "Prescriptions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TreatmentForId",
                table: "PrescriptionDetails",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TreatmentForId",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "TreatmentForId",
                table: "PrescriptionDetails");

            migrationBuilder.AddColumn<string>(
                name: "PrescriptionFor",
                table: "Prescriptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

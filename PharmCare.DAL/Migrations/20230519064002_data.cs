using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmCare.DAL.Migrations
{
    public partial class data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PrescriptionDetailId",
                table: "Patients",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PrescriptionDetailId",
                table: "Patients",
                column: "PrescriptionDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_PrescriptionDetails_PrescriptionDetailId",
                table: "Patients",
                column: "PrescriptionDetailId",
                principalTable: "PrescriptionDetails",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_PrescriptionDetails_PrescriptionDetailId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_PrescriptionDetailId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "PrescriptionDetailId",
                table: "Patients");
        }
    }
}

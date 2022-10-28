using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmCare.DAL.Migrations
{
    public partial class _347554655 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Counties_CountyId",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_SubCounties_SubCountyId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_CountyId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_SubCountyId",
                table: "Patients");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Patients_CountyId",
                table: "Patients",
                column: "CountyId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_SubCountyId",
                table: "Patients",
                column: "SubCountyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Counties_CountyId",
                table: "Patients",
                column: "CountyId",
                principalTable: "Counties",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_SubCounties_SubCountyId",
                table: "Patients",
                column: "SubCountyId",
                principalTable: "SubCounties",
                principalColumn: "Id");
        }
    }
}

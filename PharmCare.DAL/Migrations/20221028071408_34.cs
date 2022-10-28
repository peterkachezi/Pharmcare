using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmCare.DAL.Migrations
{
    public partial class _34 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medicines_Categories",
                table: "Medicines");

            migrationBuilder.DropForeignKey(
                name: "FK_Medicines_MedicalConditions",
                table: "Medicines");

            migrationBuilder.DropForeignKey(
                name: "FK_Medicines_Units",
                table: "Medicines");

            migrationBuilder.AddForeignKey(
                name: "FK_Medicines_Categories_CategoryId",
                table: "Medicines",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Medicines_MedicalConditions_MedicalConditionId",
                table: "Medicines",
                column: "MedicalConditionId",
                principalTable: "MedicalConditions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Medicines_Units_UnitId",
                table: "Medicines",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medicines_Categories_CategoryId",
                table: "Medicines");

            migrationBuilder.DropForeignKey(
                name: "FK_Medicines_MedicalConditions_MedicalConditionId",
                table: "Medicines");

            migrationBuilder.DropForeignKey(
                name: "FK_Medicines_Units_UnitId",
                table: "Medicines");

            migrationBuilder.AddForeignKey(
                name: "FK_Medicines_Categories",
                table: "Medicines",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Medicines_MedicalConditions",
                table: "Medicines",
                column: "MedicalConditionId",
                principalTable: "MedicalConditions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Medicines_Units",
                table: "Medicines",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id");
        }
    }
}

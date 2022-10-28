using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmCare.DAL.Migrations
{
    public partial class _347554 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Counties",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_SubCounties",
                table: "Patients");

            migrationBuilder.AlterColumn<Guid>(
                name: "SubCountyId",
                table: "Patients",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "CountyId",
                table: "Patients",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Counties_CountyId",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_SubCounties_SubCountyId",
                table: "Patients");

            migrationBuilder.AlterColumn<Guid>(
                name: "SubCountyId",
                table: "Patients",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CountyId",
                table: "Patients",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Counties",
                table: "Patients",
                column: "CountyId",
                principalTable: "Counties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_SubCounties",
                table: "Patients",
                column: "SubCountyId",
                principalTable: "SubCounties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

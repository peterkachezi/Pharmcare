using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmCare.DAL.Migrations
{
    public partial class _25 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Condition",
                table: "Medicines");

            migrationBuilder.AddColumn<Guid>(
                name: "MedicalConditionId",
                table: "Medicines",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MedicalConditionId",
                table: "Medicines");

            migrationBuilder.AddColumn<string>(
                name: "Condition",
                table: "Medicines",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

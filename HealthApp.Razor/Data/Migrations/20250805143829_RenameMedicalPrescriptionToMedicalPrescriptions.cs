using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthApp.Razor.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameMedicalPrescriptionToMedicalPrescriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MedicalPrescription",
                table: "MedicalPrescription");

            migrationBuilder.RenameTable(
                name: "MedicalPrescription",
                newName: "MedicalPrescriptions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MedicalPrescriptions",
                table: "MedicalPrescriptions",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MedicalPrescriptions",
                table: "MedicalPrescriptions");

            migrationBuilder.RenameTable(
                name: "MedicalPrescriptions",
                newName: "MedicalPrescription");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MedicalPrescription",
                table: "MedicalPrescription",
                column: "Id");
        }
    }
}

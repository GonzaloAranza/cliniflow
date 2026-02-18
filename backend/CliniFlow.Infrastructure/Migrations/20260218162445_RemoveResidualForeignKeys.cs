using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliniFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveResidualForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecords_Professionals_ProfessionalId",
                table: "MedicalRecords");

            migrationBuilder.DropIndex(
                name: "IX_MedicalRecords_ProfessionalId",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "ProfessionalId",
                table: "MedicalRecords");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProfessionalId",
                table: "MedicalRecords",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicalRecords_ProfessionalId",
                table: "MedicalRecords",
                column: "ProfessionalId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecords_Professionals_ProfessionalId",
                table: "MedicalRecords",
                column: "ProfessionalId",
                principalTable: "Professionals",
                principalColumn: "Id");
        }
    }
}

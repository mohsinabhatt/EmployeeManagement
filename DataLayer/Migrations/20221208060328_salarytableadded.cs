using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class salarytableadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Leave",
                table: "SalaryDeductions",
                newName: "LeaveDeductedSal");

            migrationBuilder.CreateTable(
                name: "Leaves",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LegalLeaves = table.Column<int>(type: "int", nullable: false),
                    NoOfLeaves = table.Column<int>(type: "int", nullable: false),
                    TotalLeaves = table.Column<int>(type: "int", nullable: false),
                    SalaryDeductionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leaves", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Leaves_SalaryDeductions_SalaryDeductionId",
                        column: x => x.SalaryDeductionId,
                        principalTable: "SalaryDeductions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_SalaryDeductionId",
                table: "Leaves",
                column: "SalaryDeductionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Leaves");

            migrationBuilder.RenameColumn(
                name: "LeaveDeductedSal",
                table: "SalaryDeductions",
                newName: "Leave");
        }
    }
}

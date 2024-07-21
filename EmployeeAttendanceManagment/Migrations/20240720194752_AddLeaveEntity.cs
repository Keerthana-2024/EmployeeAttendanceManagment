using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeAttendanceManagment.Migrations
{
    /// <inheritdoc />
    public partial class AddLeaveEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Leaves",
                columns: table => new
                {
                    LeaveID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeID = table.Column<int>(type: "int", nullable: false),
                    CasualLeaves = table.Column<int>(type: "int", nullable: false),
                    SickLeaves = table.Column<int>(type: "int", nullable: false),
                    ComboLeaves = table.Column<int>(type: "int", nullable: false),
                    PaidLeaves = table.Column<int>(type: "int", nullable: false),
                    TakenCasualLeaves = table.Column<int>(type: "int", nullable: false),
                    TakenSickLeaves = table.Column<int>(type: "int", nullable: false),
                    TakenComboLeaves = table.Column<int>(type: "int", nullable: false),
                    TakenPaidLeaves = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leaves", x => x.LeaveID);
                    table.ForeignKey(
                        name: "FK_Leaves_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_EmployeeID",
                table: "Leaves",
                column: "EmployeeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Leaves");
        }
    }
}

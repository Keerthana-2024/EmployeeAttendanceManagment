using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeAttendanceManagment.Migrations
{
    /// <inheritdoc />
    public partial class Policy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Region",
                table: "AttendancePolicies",
                newName: "PolicyName");

            migrationBuilder.RenameColumn(
                name: "PolicyDetails",
                table: "AttendancePolicies",
                newName: "AllowedLocations");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AttendancePolicies",
                newName: "PolicyID");

            migrationBuilder.CreateTable(
                name: "Attendance",
                columns: table => new
                {
                    AttendanceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeID = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPresent = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendance", x => x.AttendanceID);
                    table.ForeignKey(
                        name: "FK_Attendance_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_EmployeeID",
                table: "Attendance",
                column: "EmployeeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendance");

            migrationBuilder.RenameColumn(
                name: "PolicyName",
                table: "AttendancePolicies",
                newName: "Region");

            migrationBuilder.RenameColumn(
                name: "AllowedLocations",
                table: "AttendancePolicies",
                newName: "PolicyDetails");

            migrationBuilder.RenameColumn(
                name: "PolicyID",
                table: "AttendancePolicies",
                newName: "Id");
        }
    }
}

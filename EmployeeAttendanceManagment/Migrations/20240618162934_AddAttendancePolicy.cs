using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeAttendanceManagment.Migrations
{
    /// <inheritdoc />
    public partial class AddAttendancePolicy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DepartmentID",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AttendancePolicyID",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AttendancePolicies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PolicyDetails = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendancePolicies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HolidayCalendars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HolidayCalendars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationCharts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeID = table.Column<int>(type: "int", nullable: false),
                    ManagerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationCharts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationCharts_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizationCharts_Employees_ManagerID",
                        column: x => x.ManagerID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict); // Change made here
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_AttendancePolicyID",
                table: "Employees",
                column: "AttendancePolicyID");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationCharts_EmployeeID",
                table: "OrganizationCharts",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationCharts_ManagerID",
                table: "OrganizationCharts",
                column: "ManagerID");

            // Insert a default attendance policy
            migrationBuilder.Sql("INSERT INTO AttendancePolicies (Region, PolicyDetails) VALUES ('Default', 'Default Policy')");

            // Set all existing Employee's AttendancePolicyID to the default policy's Id
            migrationBuilder.Sql("UPDATE Employees SET AttendancePolicyID = (SELECT Id FROM AttendancePolicies WHERE Region = 'Default')");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_AttendancePolicies_AttendancePolicyID",
                table: "Employees",
                column: "AttendancePolicyID",
                principalTable: "AttendancePolicies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_AttendancePolicies_AttendancePolicyID",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "AttendancePolicies");

            migrationBuilder.DropTable(
                name: "HolidayCalendars");

            migrationBuilder.DropTable(
                name: "OrganizationCharts");

            migrationBuilder.DropIndex(
                name: "IX_Employees_AttendancePolicyID",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "AttendancePolicyID",
                table: "Employees");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentID",
                table: "Employees",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}

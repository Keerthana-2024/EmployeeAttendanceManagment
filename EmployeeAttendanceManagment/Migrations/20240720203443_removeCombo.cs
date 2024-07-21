using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeAttendanceManagment.Migrations
{
    /// <inheritdoc />
    public partial class removeCombo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComboLeaves",
                table: "Leaves");

            migrationBuilder.DropColumn(
                name: "TakenComboLeaves",
                table: "Leaves");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ComboLeaves",
                table: "Leaves",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TakenComboLeaves",
                table: "Leaves",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

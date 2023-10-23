using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskSoliq.Migrations
{
    /// <inheritdoc />
    public partial class updatedatabasearchitecture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEmployee",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeCategory",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeCategory",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Users");

            migrationBuilder.AddColumn<bool>(
                name: "IsEmployee",
                table: "Users",
                type: "bit",
                nullable: true);
        }
    }
}

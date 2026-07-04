using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School_Project_API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalHomewoks",
                table: "CourseProgress",
                newName: "TotalHomeworks");

            migrationBuilder.RenameColumn(
                name: "HomewokScore",
                table: "CourseProgress",
                newName: "HomeworkScore");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalHomeworks",
                table: "CourseProgress",
                newName: "TotalHomewoks");

            migrationBuilder.RenameColumn(
                name: "HomeworkScore",
                table: "CourseProgress",
                newName: "HomewokScore");
        }
    }
}

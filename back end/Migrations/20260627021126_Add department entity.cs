using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School_Project_API.Migrations
{
    /// <inheritdoc />
    public partial class Adddepartmententity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Subjects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Classes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1,
                column: "DepartmentId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2,
                column: "DepartmentId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 3,
                column: "DepartmentId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 4,
                column: "DepartmentId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 5,
                column: "DepartmentId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 6,
                column: "DepartmentId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 7,
                column: "DepartmentId",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 8,
                column: "DepartmentId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 9,
                column: "DepartmentId",
                value: 9);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 10,
                column: "DepartmentId",
                value: 10);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 1,
                column: "DepartmentId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 2,
                column: "DepartmentId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 3,
                column: "DepartmentId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 4,
                column: "DepartmentId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 5,
                column: "DepartmentId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 6,
                column: "DepartmentId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 7,
                column: "DepartmentId",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 8,
                column: "DepartmentId",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 9,
                column: "DepartmentId",
                value: 9);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 10,
                column: "DepartmentId",
                value: 4);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_DepartmentId",
                table: "Subjects",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_DepartmentId",
                table: "Classes",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Departments_DepartmentId",
                table: "Classes",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Departments_DepartmentId",
                table: "Subjects",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Departments_DepartmentId",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Departments_DepartmentId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_DepartmentId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Classes_DepartmentId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Classes");
        }
    }
}

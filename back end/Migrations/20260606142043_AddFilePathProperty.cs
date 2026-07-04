using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School_Project_API.Migrations
{
    /// <inheritdoc />
    public partial class AddFilePathProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Homeworks",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Homeworks",
                keyColumn: "Id",
                keyValue: 1,
                column: "FilePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Homeworks",
                keyColumn: "Id",
                keyValue: 2,
                column: "FilePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Homeworks",
                keyColumn: "Id",
                keyValue: 3,
                column: "FilePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Homeworks",
                keyColumn: "Id",
                keyValue: 4,
                column: "FilePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Homeworks",
                keyColumn: "Id",
                keyValue: 5,
                column: "FilePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Homeworks",
                keyColumn: "Id",
                keyValue: 6,
                column: "FilePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Homeworks",
                keyColumn: "Id",
                keyValue: 7,
                column: "FilePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Homeworks",
                keyColumn: "Id",
                keyValue: 8,
                column: "FilePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Homeworks",
                keyColumn: "Id",
                keyValue: 9,
                column: "FilePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Homeworks",
                keyColumn: "Id",
                keyValue: 10,
                column: "FilePath",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Homeworks");
        }
    }
}

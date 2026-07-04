using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School_Project_API.Migrations
{
    /// <inheritdoc />
    public partial class AddAttendanceAlertEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AttendanceAlerts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: true),
                    ConsecutiveAbsences = table.Column<int>(type: "int", nullable: false),
                    AlertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceAlerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendanceAlerts_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceAlerts_AlertDate",
                table: "AttendanceAlerts",
                column: "AlertDate");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceAlerts_Status",
                table: "AttendanceAlerts",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceAlerts_StudentId",
                table: "AttendanceAlerts",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttendanceAlerts");
        }
    }
}

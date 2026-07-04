using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace School_Project_API.Migrations
{
    /// <inheritdoc />
    public partial class AddexamandprogressCourseentities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseProgress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    HomewokScore = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    AttendanceScore = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    ExamScore = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    OverallProgress = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    CalculatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    TotalHomewoks = table.Column<int>(type: "int", nullable: false),
                    TotalAttendanceDays = table.Column<int>(type: "int", nullable: false),
                    TotalExams = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseProgress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseProgress_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseProgress_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Exams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: true),
                    CourseId = table.Column<int>(type: "int", nullable: true),
                    Score = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    MaxScore = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 100m),
                    ExamDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exams_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Exams_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "Exams",
                columns: new[] { "Id", "CourseId", "ExamDate", "MaxScore", "Notes", "Score", "StudentId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 100m, null, 85m, 1 },
                    { 2, 1, new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 100m, null, 72m, 2 },
                    { 3, 2, new DateTime(2026, 5, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 100m, null, 90m, 3 },
                    { 4, 2, new DateTime(2026, 5, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 100m, null, 78m, 1 },
                    { 5, 1, new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 100m, null, 65m, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseProgress_CourseId",
                table: "CourseProgress",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseProgress_StudentId_CourseId",
                table: "CourseProgress",
                columns: new[] { "StudentId", "CourseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exams_CourseId",
                table: "Exams",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_StudentId",
                table: "Exams",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseProgress");

            migrationBuilder.DropTable(
                name: "Exams");
        }
    }
}

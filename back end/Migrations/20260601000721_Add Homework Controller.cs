using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace School_Project_API.Migrations
{
    /// <inheritdoc />
    public partial class AddHomeworkController : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Homeworks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: true),
                    SubjectId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Homeworks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Homeworks_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Homeworks_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Homeworks_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Homeworks",
                columns: new[] { "Id", "ClassId", "CreatedAt", "Description", "DueDate", "Status", "SubjectId", "TeacherId", "Title" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Complete exercises 1 to 15 from the algebra worksheet.", new DateTime(2026, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, 1, "Math Algebra Practice" },
                    { 2, 2, new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Write a lab report about the plant growth experiment.", new DateTime(2026, 6, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, 1, "Science Lab Report" },
                    { 3, 1, new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Read chapter 4 and write a one-page summary.", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 3, 2, "English Reading Summary" },
                    { 4, 3, new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Create a timeline of key events from the lesson.", new DateTime(2026, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 4, 2, "History Timeline" },
                    { 5, 2, new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Label the countries and capitals on the provided map.", new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 5, 3, "Geography Map Activity" },
                    { 6, 4, new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Answer the questions about hardware and software.", new DateTime(2026, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 6, 3, "Computer Basics Assignment" },
                    { 7, 3, new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Complete the grammar exercises from page 32.", new DateTime(2026, 6, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 7, 4, "Arabic Grammar Practice" },
                    { 8, 4, new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Draw a still-life sketch using pencil shading.", new DateTime(2026, 6, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 8, 4, "Art Sketch Assignment" },
                    { 9, 5, new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Write a short reflection about teamwork in sports.", new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 9, 5, "Physical Education Reflection" },
                    { 10, 5, new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Complete the revision worksheet before next class.", new DateTime(2026, 6, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 10, 5, "Final Revision Worksheet" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Homeworks_ClassId",
                table: "Homeworks",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Homeworks_DueDate",
                table: "Homeworks",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_Homeworks_SubjectId",
                table: "Homeworks",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Homeworks_TeacherId",
                table: "Homeworks",
                column: "TeacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Homeworks");
        }
    }
}

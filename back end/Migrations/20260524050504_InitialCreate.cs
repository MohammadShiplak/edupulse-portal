using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace School_Project_API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccessCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SerialNo = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessCards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CardId = table.Column<int>(type: "int", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_AccessCards_CardId",
                        column: x => x.CardId,
                        principalTable: "AccessCards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Students_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Specialization = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teachers_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attendances_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false, defaultValue: "No Description")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Classes_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    SubjectName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subjects_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subjects_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentSubjects",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    EnrollmentDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Grade = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentSubjects", x => new { x.StudentId, x.SubjectId });
                    table.ForeignKey(
                        name: "FK_StudentSubjects_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentSubjects_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AccessCards",
                columns: new[] { "Id", "ExpirationDate", "SerialNo" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "ABCAW" },
                    { 2, new DateTime(2025, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "ABCAX" },
                    { 3, new DateTime(2025, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "ABCBW" },
                    { 4, new DateTime(2025, 5, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "ABCBX" },
                    { 5, new DateTime(2025, 6, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "ABCCA" },
                    { 6, new DateTime(2025, 7, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "ABCCX" },
                    { 7, new DateTime(2025, 8, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "ABCDW" },
                    { 8, new DateTime(2025, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "ABCDX" },
                    { 9, new DateTime(2025, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "ABCDE" },
                    { 10, new DateTime(2025, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "ABCDF" },
                    { 11, new DateTime(2025, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "ABCDG" },
                    { 12, new DateTime(2026, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "ABCDH" },
                    { 13, new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "ABCDI" }
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "ImagePath", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "/images/asp.png", "ASP.NET Core", 100m },
                    { 2, "/images/react.png", "React.js", 120m },
                    { 3, "/images/sql.png", "SQL Server", 80m },
                    { 4, "/images/docker.png", "Docker", 90m },
                    { 5, "/images/azure.png", "Azure", 110m },
                    { 6, "/images/python.png", "Python", 95m },
                    { 7, "/images/security.png", "Cyber Security", 130m },
                    { 8, "/images/ml.png", "Machine Learning", 150m },
                    { 9, "/images/network.png", "Networking", 85m },
                    { 10, "/images/algo.png", "Algorithms", 140m }
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "IT" },
                    { 2, "HR" },
                    { 3, "Finance" },
                    { 4, "Marketing" },
                    { 5, "Sales" },
                    { 6, "Operations" },
                    { 7, "Cyber Security" },
                    { 8, "AI" },
                    { 9, "Networking" },
                    { 10, "Business" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "IsActive", "PasswordHash", "Role", "UserName" },
                values: new object[,]
                {
                    { 1, "admin@gmail.com", true, "$2a$11$c.4Cmj3rP.Zrpq9PD0ZZveE/aMFE504E9hxqbVVWKesvuDwElKksu", "Admin", "admin" },
                    { 2, "teacher1@gmail.com", true, "$2a$11$BwOhy/TISJ6FY796dT0I.eN5SALsWuTs6JRp9sRtD0bpXcwEYojtS", "Teacher", "teacher1" },
                    { 3, "teacher2@gmail.com", true, "$2a$11$AnlrDcQ1SMmjaQd9WMr8RO1/IC2g9fUdx9TOAMzorCCC7D/OcMHLa", "Teacher", "teacher2" },
                    { 4, "student1@gmail.com", true, "$2a$11$3Q7ktMT/zai/u.w7PHB7h.2h28yrTIKaLnLDaB0kYfQqAr2ZUSevK", "Student", "student1" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "PasswordHash", "Role", "UserName" },
                values: new object[] { 5, "student2@gmail.com", "$2a$11$vSO5US.IN.S5qC.o4Xy91eU9YnOJ/zhPhEE8hqXP9O1/kgqJv3I4S", "Student", "student2" });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "Address", "CardId", "DateOfBirth", "DepartmentId", "Email", "FirstName", "Gender", "LastName", "Phone" },
                values: new object[,]
                {
                    { 1, "Amman", 1, new DateTime(2003, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "m1@test.com", "Mohammad", "Male", "Shiplak", "111" },
                    { 2, "Irbid", 2, new DateTime(2002, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "m2@test.com", "Ahmad", "Male", "Ali", "222" },
                    { 3, "Zarqa", 3, new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "m3@test.com", "Sara", "Female", "Khaled", "333" },
                    { 4, "Aqaba", 4, new DateTime(2000, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "m4@test.com", "Lina", "Female", "Hasan", "444" },
                    { 5, "Madaba", 5, new DateTime(1999, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, "m5@test.com", "Omar", "Male", "Salem", "555" },
                    { 6, "Salt", 6, new DateTime(2003, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, "m6@test.com", "Rami", "Male", "Naser", "666" },
                    { 7, "Karak", 7, new DateTime(2002, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, "m7@test.com", "Noor", "Female", "Sami", "777" },
                    { 8, "Jerash", 8, new DateTime(2001, 8, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, "m8@test.com", "Yousef", "Male", "Adel", "888" },
                    { 9, "Ajloun", 9, new DateTime(2000, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, "m9@test.com", "Mona", "Female", "Ahmad", "999" },
                    { 10, "Mafraq", 10, new DateTime(1998, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, "m10@test.com", "Khaled", "Male", "Jamal", "1010" }
                });

            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "Id", "DepartmentId", "HireDate", "Name", "Specialization" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ali Ahmad", "Backend" },
                    { 2, 1, new DateTime(2021, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sara Khaled", "Frontend" },
                    { 3, 2, new DateTime(2019, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Omar Sami", "HR" },
                    { 4, 3, new DateTime(2022, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lina Hasan", "Finance" },
                    { 5, 4, new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ahmad Naser", "Marketing" },
                    { 6, 5, new DateTime(2018, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yousef Adel", "Sales" },
                    { 7, 6, new DateTime(2023, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mona Ali", "Operations" },
                    { 8, 7, new DateTime(2021, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Khaled Jamal", "Cyber Security" },
                    { 9, 8, new DateTime(2019, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rami Saeed", "AI" },
                    { 10, 9, new DateTime(2022, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Noor Hasan", "Networking" }
                });

            migrationBuilder.InsertData(
                table: "Attendances",
                columns: new[] { "Id", "Date", "Notes", "Status", "StudentId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "On time", 1, 1 },
                    { 2, new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sick", 2, 2 },
                    { 3, new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Traffic", 3, 3 },
                    { 4, new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Excellent", 1, 4 },
                    { 5, new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Medical excuse", 4, 5 },
                    { 6, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "On time", 1, 6 },
                    { 7, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Travel", 2, 7 },
                    { 8, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Good", 1, 8 },
                    { 9, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Late bus", 3, 9 },
                    { 10, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Participated", 1, 10 }
                });

            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "Id", "Capacity", "Description", "Name", "TeacherId" },
                values: new object[,]
                {
                    { 1, 30, "Backend", "Class A", 1 },
                    { 2, 25, "Frontend", "Class B", 2 },
                    { 3, 20, "HR", "Class C", 3 },
                    { 4, 35, "Finance", "Class D", 4 },
                    { 5, 40, "Marketing", "Class E", 5 },
                    { 6, 28, "Sales", "Class F", 6 },
                    { 7, 32, "Operations", "Class G", 7 },
                    { 8, 18, "Security", "Class H", 8 },
                    { 9, 22, "AI", "Class I", 9 },
                    { 10, 26, "Networking", "Class J", 10 }
                });

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "Id", "CourseId", "Price", "SubjectName", "TeacherId" },
                values: new object[,]
                {
                    { 1, 1, 50m, "C#", 1 },
                    { 2, 1, 60m, "EF Core", 1 },
                    { 3, 2, 40m, "HTML", 2 },
                    { 4, 2, 40m, "CSS", 2 },
                    { 5, 3, 55m, "SQL Basics", 4 },
                    { 6, 4, 65m, "Docker Basics", 7 },
                    { 7, 5, 75m, "Azure Fundamentals", 9 },
                    { 8, 6, 45m, "Python Basics", 9 },
                    { 9, 7, 90m, "Ethical Hacking", 8 },
                    { 10, 10, 100m, "Data Structures", 10 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_StudentId_Date",
                table: "Attendances",
                columns: new[] { "StudentId", "Date" },
                unique: true,
                filter: "[StudentId] IS NOT NULL AND [Date] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_TeacherId",
                table: "Classes",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_CardId",
                table: "Students",
                column: "CardId",
                unique: true,
                filter: "[CardId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Students_DepartmentId",
                table: "Students",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentSubjects_SubjectId",
                table: "StudentSubjects",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_CourseId",
                table: "Subjects",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_TeacherId",
                table: "Subjects",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_DepartmentId",
                table: "Teachers",
                column: "DepartmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "StudentSubjects");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "AccessCards");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}

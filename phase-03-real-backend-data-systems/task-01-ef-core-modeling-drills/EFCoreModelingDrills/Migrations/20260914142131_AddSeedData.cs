using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace task_01_drills.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Instructors",
                columns: new[] { "Id", "FullName" },
                values: new object[,]
                {
                    { 101, "Ahmed Samir" },
                    { 102, "Nour Khaled" }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "CreatedAt", "Email", "FullName", "IsActive" },
                values: new object[,]
                {
                    { 101, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "ahmed.ali@techmaster.com", "Ahmed Ali", true },
                    { 102, new DateTime(2026, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "mona.hassan@techmaster.com", "Mona Hassan", true },
                    { 103, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "omar.mohamed@techmaster.com", "Omar Mohamed", true },
                    { 104, new DateTime(2026, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "sara.mahmoud@techmaster.com", "Sara Mahmoud", true },
                    { 105, new DateTime(2026, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "youssef.ibrahim@techmaster.com", "Youssef Ibrahim", true }
                });

            migrationBuilder.InsertData(
                table: "Tracks",
                columns: new[] { "Id", "InstructorId", "Name" },
                values: new object[,]
                {
                    { 101, 101, "ASP.NET Core Backend" },
                    { 102, 101, "Database & EF Core" },
                    { 103, 102, "Web API Development" }
                });

            migrationBuilder.InsertData(
                table: "Enrollments",
                columns: new[] { "Id", "EnrollmentDate", "FinalGrade", "Status", "StudentId", "TrackId" },
                values: new object[,]
                {
                    { 101, new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Active", 101, 101 },
                    { 102, new DateTime(2026, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 92.50m, "Completed", 101, 102 },
                    { 103, new DateTime(2026, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Active", 102, 101 },
                    { 104, new DateTime(2026, 2, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 88.00m, "Completed", 103, 103 },
                    { 105, new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Active", 104, 102 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Instructors",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Instructors",
                keyColumn: "Id",
                keyValue: 102);
        }
    }
}

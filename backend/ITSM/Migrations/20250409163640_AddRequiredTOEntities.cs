using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITSM.Migrations
{
    /// <inheritdoc />
    public partial class AddRequiredTOEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$OQyawnrLQU51AiOHU95o5eQxsrVxEhh/LuBUJfUDEq48VQTQaG6ni");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$OQyawnrLQU51AiOHU95o5eQxsrVxEhh/LuBUJfUDEq48VQTQaG6ni");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "Password",
                value: "$2a$11$OQyawnrLQU51AiOHU95o5eQxsrVxEhh/LuBUJfUDEq48VQTQaG6ni");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "Password",
                value: "$2a$11$OQyawnrLQU51AiOHU95o5eQxsrVxEhh/LuBUJfUDEq48VQTQaG6ni");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "Password",
                value: "$2a$11$OQyawnrLQU51AiOHU95o5eQxsrVxEhh/LuBUJfUDEq48VQTQaG6ni");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$gLkNA5KWnUXXyYxexBoSxOBQjRQMFZHFCQmZ.1nMBS32HIrjZY1W2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$gLkNA5KWnUXXyYxexBoSxOBQjRQMFZHFCQmZ.1nMBS32HIrjZY1W2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "Password",
                value: "$2a$11$KbPdkb4tRn7/8JFGYOHMHe0dKa0aZpMLMzZxeUpPRi9kXygKgMY0K");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "Password",
                value: "$2a$11$KbPdkb4tRn7/8JFGYOHMHe0dKa0aZpMLMzZxeUpPRi9kXygKgMY0K");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "Password",
                value: "$2a$11$KbPdkb4tRn7/8JFGYOHMHe0dKa0aZpMLMzZxeUpPRi9kXygKgMY0K");
        }
    }
}

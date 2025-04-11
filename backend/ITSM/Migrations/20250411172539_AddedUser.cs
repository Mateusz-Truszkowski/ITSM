using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITSM.Migrations
{
    /// <inheritdoc />
    public partial class AddedUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreationDate", "Email", "Group", "Login", "Name", "Occupation", "Password", "Status", "Surname" },
                values: new object[] { 6, new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "karolpajako@gmail.com", "Admin", "Daniel", "Daniel", "Software Engineer", "$2a$11$OQyawnrLQU51AiOHU95o5eQxsrVxEhh/LuBUJfUDEq48VQTQaG6ni", "Active", "Suski" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}

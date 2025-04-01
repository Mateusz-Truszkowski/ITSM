using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ITSM.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContractingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SLA = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Group = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Occupation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "ContractingDate", "Description", "Name", "SLA", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Providing reliable and fast web hosting services.", "Web Hosting", 99, "Active" },
                    { 2, new DateTime(2022, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Scalable cloud storage solutions for businesses of all sizes.", "Cloud Storage", 99, "Active" },
                    { 3, new DateTime(2023, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Email marketing campaigns, automation, and reporting.", "Email Marketing", 98, "Inactive" },
                    { 4, new DateTime(2022, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Secure backup and disaster recovery solutions.", "Data Backup", 97, "Active" },
                    { 5, new DateTime(2023, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Search engine optimization to increase website visibility.", "SEO Optimization", 95, "Active" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreationDate", "Email", "Group", "Login", "Name", "Occupation", "Password", "Status", "Surname" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "jdoe@example.com", "Admin", "jdoe", "John", "Software Engineer", "Password123", "Active", "Doe" },
                    { 2, new DateTime(2022, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "asmith@example.com", "User", "asmith", "Alice", "Product Manager", "SecurePass456", "Active", "Smith" },
                    { 3, new DateTime(2023, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "bjohnson@example.com", "Moderator", "bjohnson", "Bob", "QA Engineer", "TestPass789", "Inactive", "Johnson" },
                    { 4, new DateTime(2022, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "klane@example.com", "User", "klane", "Kate", "UX Designer", "KatePass999", "Active", "Lane" },
                    { 5, new DateTime(2023, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "mwhite@example.com", "Admin", "mwhite", "Michael", "CTO", "MikePass111", "Suspended", "White" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ITSM.Migrations
{
    /// <inheritdoc />
    public partial class AddTicketsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SolutionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SolutionDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    RequesterId = table.Column<int>(type: "int", nullable: false),
                    AssigneeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_Users_AssigneeId",
                        column: x => x.AssigneeId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_Users_RequesterId",
                        column: x => x.RequesterId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "AssigneeId", "CreationDate", "Description", "Name", "Priority", "RequesterId", "ServiceId", "SolutionDate", "SolutionDescription", "Status", "Type" },
                values: new object[,]
                {
                    { 1, 2, new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User is unable to log into the system. Error message: 'Invalid credentials.'", "Issue with login", 2, 1, 1, null, null, "Open", "Bug" },
                    { 2, 3, new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "The application is lagging when loading the dashboard.", "System performance issue", 1, 2, 2, null, null, "In Progress", "Performance" },
                    { 3, 1, new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User has forgotten their password and needs a reset.", "Password reset request", 3, 3, 3, new DateTime(2025, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Password was successfully reset and communicated to the user.", "Closed", "Support" },
                    { 4, 3, new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "A user requests the addition of a dark mode feature in the app.", "New feature request", 4, 4, 1, null, null, "Open", "Feature Request" },
                    { 5, 2, new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User is receiving an error when trying to update their profile information.", "Error while updating profile", 2, 5, 2, new DateTime(2025, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "The issue was resolved by fixing the validation error in the form.", "Closed", "Bug" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_AssigneeId",
                table: "Tickets",
                column: "AssigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_RequesterId",
                table: "Tickets",
                column: "RequesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ServiceId",
                table: "Tickets",
                column: "ServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tickets");
        }
    }
}

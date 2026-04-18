using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SomaliskDanskForening_Lib.Migrations
{
    /// <inheritdoc />
    public partial class CleanUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "IsActive", "PasswordHash", "Role", "Username" },
                values: new object[] { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@forening.dk", true, "L8mN2oP5qR8sT1uV4wX7yZ0aB3cD6eF9gH2iJ5kL8mN1oP4qR7sT0uV3wX6yZ9aB", "Admin", "SomaliskDanskForening" });
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyApi.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Employees",
                newName: "HashPassword");

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "Employees",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "HashPassword",
                table: "Employees",
                newName: "Password");
        }
    }
}

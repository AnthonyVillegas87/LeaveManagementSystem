using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagementSystem.Web.Migrations
{
    /// <inheritdoc />
    public partial class ExtendedUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "df7cd3e7-b3c2-44d0-9027-aad95c5b89c5",
                columns: new[] { "ConcurrencyStamp", "DateOfBirth", "FirstName", "LastName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "940c157e-ca76-4181-9f7f-7a9172abe247", new DateOnly(1980, 1, 1), "Default", "Admin", "AQAAAAIAAYagAAAAEKm8hyheYc/nV1wDD1lvAigdhLePI0YBIOQJ76mVaz8ysNTxKGk4NKK+YcTpt8D4ew==", "5d6532ec-f5d7-475e-a05d-5dd16f8be0a7" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "df7cd3e7-b3c2-44d0-9027-aad95c5b89c5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0f03c6fd-dba1-4ae1-8418-bd089543a863", "AQAAAAIAAYagAAAAEIhX3dvTDTN3WDuztgaK4utD44SOkfE81LFBXSJ7Qfju9lytsyZ4tTjY7NsbZXlKvg==", "9164492b-7f06-460c-b26b-0b70ee034c8e" });
        }
    }
}

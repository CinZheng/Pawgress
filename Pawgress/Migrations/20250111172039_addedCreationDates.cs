using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pawgress.Migrations
{
    /// <inheritdoc />
    public partial class addedCreationDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Notes",
                newName: "CreationDate");

            migrationBuilder.RenameColumn(
                name: "RecordedDate",
                table: "DogSensorData",
                newName: "CreationDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Quizzes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Lessons",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Lessons");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "Notes",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "DogSensorData",
                newName: "RecordedDate");
        }
    }
}

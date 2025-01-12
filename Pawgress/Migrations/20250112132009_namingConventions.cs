using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pawgress.Migrations
{
    /// <inheritdoc />
    public partial class namingConventions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuizDescription",
                table: "TrainingPathItems");

            migrationBuilder.DropColumn(
                name: "QuizId",
                table: "TrainingPathItems");

            migrationBuilder.RenameColumn(
                name: "QuizName",
                table: "TrainingPathItems",
                newName: "Description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "TrainingPathItems",
                newName: "QuizName");

            migrationBuilder.AddColumn<string>(
                name: "QuizDescription",
                table: "TrainingPathItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "QuizId",
                table: "TrainingPathItems",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}

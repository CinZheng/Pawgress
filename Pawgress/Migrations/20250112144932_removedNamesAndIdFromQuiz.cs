using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pawgress.Migrations
{
    /// <inheritdoc />
    public partial class removedNamesAndIdFromQuiz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lesson_SortOrder",
                table: "TrainingPathItems");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "TrainingPathItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Lesson_SortOrder",
                table: "TrainingPathItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "TrainingPathItems",
                type: "int",
                nullable: true);
        }
    }
}

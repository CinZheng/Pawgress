using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pawgress.Migrations
{
    /// <inheritdoc />
    public partial class manytomanyquizlessons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingPathItems_Libraries_Lesson_LibraryId",
                table: "TrainingPathItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingPathItems_Libraries_LibraryId",
                table: "TrainingPathItems");

            migrationBuilder.DropIndex(
                name: "IX_TrainingPathItems_Lesson_LibraryId",
                table: "TrainingPathItems");

            migrationBuilder.DropIndex(
                name: "IX_TrainingPathItems_LibraryId",
                table: "TrainingPathItems");

            migrationBuilder.DropColumn(
                name: "Lesson_LibraryId",
                table: "TrainingPathItems");

            migrationBuilder.DropColumn(
                name: "LibraryId",
                table: "TrainingPathItems");

            migrationBuilder.CreateTable(
                name: "LibraryLesson",
                columns: table => new
                {
                    LibraryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LessonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryLesson", x => new { x.LibraryId, x.LessonId });
                    table.ForeignKey(
                        name: "FK_LibraryLesson_Libraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Libraries",
                        principalColumn: "LibraryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LibraryLesson_TrainingPathItems_LessonId",
                        column: x => x.LessonId,
                        principalTable: "TrainingPathItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LibraryQuiz",
                columns: table => new
                {
                    LibraryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuizId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryQuiz", x => new { x.LibraryId, x.QuizId });
                    table.ForeignKey(
                        name: "FK_LibraryQuiz_Libraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Libraries",
                        principalColumn: "LibraryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LibraryQuiz_TrainingPathItems_QuizId",
                        column: x => x.QuizId,
                        principalTable: "TrainingPathItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LibraryLesson_LessonId",
                table: "LibraryLesson",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryQuiz_QuizId",
                table: "LibraryQuiz",
                column: "QuizId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LibraryLesson");

            migrationBuilder.DropTable(
                name: "LibraryQuiz");

            migrationBuilder.AddColumn<Guid>(
                name: "Lesson_LibraryId",
                table: "TrainingPathItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LibraryId",
                table: "TrainingPathItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPathItems_Lesson_LibraryId",
                table: "TrainingPathItems",
                column: "Lesson_LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPathItems_LibraryId",
                table: "TrainingPathItems",
                column: "LibraryId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingPathItems_Libraries_Lesson_LibraryId",
                table: "TrainingPathItems",
                column: "Lesson_LibraryId",
                principalTable: "Libraries",
                principalColumn: "LibraryId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingPathItems_Libraries_LibraryId",
                table: "TrainingPathItems",
                column: "LibraryId",
                principalTable: "Libraries",
                principalColumn: "LibraryId");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pawgress.Migrations
{
    /// <inheritdoc />
    public partial class trainingpathitem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Quizzes_QuizId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Folders_FolderId",
                table: "Quizzes");

            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Libraries_LibraryId",
                table: "Quizzes");

            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_TrainingPaths_TrainingPathId",
                table: "Quizzes");

            migrationBuilder.DropTable(
                name: "Lessons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Quizzes",
                table: "Quizzes");

            migrationBuilder.RenameTable(
                name: "Quizzes",
                newName: "TrainingPathItems");

            migrationBuilder.RenameIndex(
                name: "IX_Quizzes_TrainingPathId",
                table: "TrainingPathItems",
                newName: "IX_TrainingPathItems_TrainingPathId");

            migrationBuilder.RenameIndex(
                name: "IX_Quizzes_LibraryId",
                table: "TrainingPathItems",
                newName: "IX_TrainingPathItems_LibraryId");

            migrationBuilder.RenameIndex(
                name: "IX_Quizzes_FolderId",
                table: "TrainingPathItems",
                newName: "IX_TrainingPathItems_FolderId");

            migrationBuilder.AlterColumn<int>(
                name: "SortOrder",
                table: "TrainingPathItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "QuizName",
                table: "TrainingPathItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "QuizDescription",
                table: "TrainingPathItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "QuizId",
                table: "TrainingPathItems",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "TrainingPathItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "TrainingPathItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemType",
                table: "TrainingPathItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "Lesson_FolderId",
                table: "TrainingPathItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Lesson_LibraryId",
                table: "TrainingPathItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Lesson_SortOrder",
                table: "TrainingPathItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MarkdownContent",
                table: "TrainingPathItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MediaUrl",
                table: "TrainingPathItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "TrainingPathItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Tag",
                table: "TrainingPathItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "TrainingPathItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Video",
                table: "TrainingPathItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrainingPathItems",
                table: "TrainingPathItems",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPathItems_Lesson_FolderId",
                table: "TrainingPathItems",
                column: "Lesson_FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPathItems_Lesson_LibraryId",
                table: "TrainingPathItems",
                column: "Lesson_LibraryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_TrainingPathItems_QuizId",
                table: "Questions",
                column: "QuizId",
                principalTable: "TrainingPathItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingPathItems_Folders_FolderId",
                table: "TrainingPathItems",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "FolderId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingPathItems_Folders_Lesson_FolderId",
                table: "TrainingPathItems",
                column: "Lesson_FolderId",
                principalTable: "Folders",
                principalColumn: "FolderId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingPathItems_TrainingPaths_TrainingPathId",
                table: "TrainingPathItems",
                column: "TrainingPathId",
                principalTable: "TrainingPaths",
                principalColumn: "TrainingPathId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_TrainingPathItems_QuizId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingPathItems_Folders_FolderId",
                table: "TrainingPathItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingPathItems_Folders_Lesson_FolderId",
                table: "TrainingPathItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingPathItems_Libraries_Lesson_LibraryId",
                table: "TrainingPathItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingPathItems_Libraries_LibraryId",
                table: "TrainingPathItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingPathItems_TrainingPaths_TrainingPathId",
                table: "TrainingPathItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TrainingPathItems",
                table: "TrainingPathItems");

            migrationBuilder.DropIndex(
                name: "IX_TrainingPathItems_Lesson_FolderId",
                table: "TrainingPathItems");

            migrationBuilder.DropIndex(
                name: "IX_TrainingPathItems_Lesson_LibraryId",
                table: "TrainingPathItems");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TrainingPathItems");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "TrainingPathItems");

            migrationBuilder.DropColumn(
                name: "ItemType",
                table: "TrainingPathItems");

            migrationBuilder.DropColumn(
                name: "Lesson_FolderId",
                table: "TrainingPathItems");

            migrationBuilder.DropColumn(
                name: "Lesson_LibraryId",
                table: "TrainingPathItems");

            migrationBuilder.DropColumn(
                name: "Lesson_SortOrder",
                table: "TrainingPathItems");

            migrationBuilder.DropColumn(
                name: "MarkdownContent",
                table: "TrainingPathItems");

            migrationBuilder.DropColumn(
                name: "MediaUrl",
                table: "TrainingPathItems");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "TrainingPathItems");

            migrationBuilder.DropColumn(
                name: "Tag",
                table: "TrainingPathItems");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "TrainingPathItems");

            migrationBuilder.DropColumn(
                name: "Video",
                table: "TrainingPathItems");

            migrationBuilder.RenameTable(
                name: "TrainingPathItems",
                newName: "Quizzes");

            migrationBuilder.RenameIndex(
                name: "IX_TrainingPathItems_TrainingPathId",
                table: "Quizzes",
                newName: "IX_Quizzes_TrainingPathId");

            migrationBuilder.RenameIndex(
                name: "IX_TrainingPathItems_LibraryId",
                table: "Quizzes",
                newName: "IX_Quizzes_LibraryId");

            migrationBuilder.RenameIndex(
                name: "IX_TrainingPathItems_FolderId",
                table: "Quizzes",
                newName: "IX_Quizzes_FolderId");

            migrationBuilder.AlterColumn<int>(
                name: "SortOrder",
                table: "Quizzes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QuizName",
                table: "Quizzes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "QuizId",
                table: "Quizzes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QuizDescription",
                table: "Quizzes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Quizzes",
                table: "Quizzes",
                column: "QuizId");

            migrationBuilder.CreateTable(
                name: "Lessons",
                columns: table => new
                {
                    LessonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrainingPathId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LibraryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MarkdownContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MediaUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    Tag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Video = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.LessonId);
                    table.ForeignKey(
                        name: "FK_Lessons_Folders_FolderId",
                        column: x => x.FolderId,
                        principalTable: "Folders",
                        principalColumn: "FolderId");
                    table.ForeignKey(
                        name: "FK_Lessons_Libraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Libraries",
                        principalColumn: "LibraryId");
                    table.ForeignKey(
                        name: "FK_Lessons_TrainingPaths_TrainingPathId",
                        column: x => x.TrainingPathId,
                        principalTable: "TrainingPaths",
                        principalColumn: "TrainingPathId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_FolderId",
                table: "Lessons",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_LibraryId",
                table: "Lessons",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_TrainingPathId",
                table: "Lessons",
                column: "TrainingPathId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Quizzes_QuizId",
                table: "Questions",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "QuizId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Folders_FolderId",
                table: "Quizzes",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "FolderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Libraries_LibraryId",
                table: "Quizzes",
                column: "LibraryId",
                principalTable: "Libraries",
                principalColumn: "LibraryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_TrainingPaths_TrainingPathId",
                table: "Quizzes",
                column: "TrainingPathId",
                principalTable: "TrainingPaths",
                principalColumn: "TrainingPathId");
        }
    }
}

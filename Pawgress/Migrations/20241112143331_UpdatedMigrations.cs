using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pawgress.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pages_Folders_FolderId",
                table: "Pages");

            migrationBuilder.DropForeignKey(
                name: "FK_Pages_Lessons_LessonId",
                table: "Pages");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizOption_QuizPages_QuizPageId",
                table: "QuizOption");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizPages_Quizzes_QuizId",
                table: "QuizPages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuizPages",
                table: "QuizPages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pages",
                table: "Pages");

            migrationBuilder.RenameTable(
                name: "QuizPages",
                newName: "QuizPage");

            migrationBuilder.RenameTable(
                name: "Pages",
                newName: "Page");

            migrationBuilder.RenameIndex(
                name: "IX_QuizPages_QuizId",
                table: "QuizPage",
                newName: "IX_QuizPage_QuizId");

            migrationBuilder.RenameIndex(
                name: "IX_Pages_LessonId",
                table: "Page",
                newName: "IX_Page_LessonId");

            migrationBuilder.RenameIndex(
                name: "IX_Pages_FolderId",
                table: "Page",
                newName: "IX_Page_FolderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuizPage",
                table: "QuizPage",
                column: "QuizPageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Page",
                table: "Page",
                column: "PageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Page_Folders_FolderId",
                table: "Page",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "FolderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Page_Lessons_LessonId",
                table: "Page",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "LessonId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizOption_QuizPage_QuizPageId",
                table: "QuizOption",
                column: "QuizPageId",
                principalTable: "QuizPage",
                principalColumn: "QuizPageId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizPage_Quizzes_QuizId",
                table: "QuizPage",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "QuizId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Page_Folders_FolderId",
                table: "Page");

            migrationBuilder.DropForeignKey(
                name: "FK_Page_Lessons_LessonId",
                table: "Page");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizOption_QuizPage_QuizPageId",
                table: "QuizOption");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizPage_Quizzes_QuizId",
                table: "QuizPage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuizPage",
                table: "QuizPage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Page",
                table: "Page");

            migrationBuilder.RenameTable(
                name: "QuizPage",
                newName: "QuizPages");

            migrationBuilder.RenameTable(
                name: "Page",
                newName: "Pages");

            migrationBuilder.RenameIndex(
                name: "IX_QuizPage_QuizId",
                table: "QuizPages",
                newName: "IX_QuizPages_QuizId");

            migrationBuilder.RenameIndex(
                name: "IX_Page_LessonId",
                table: "Pages",
                newName: "IX_Pages_LessonId");

            migrationBuilder.RenameIndex(
                name: "IX_Page_FolderId",
                table: "Pages",
                newName: "IX_Pages_FolderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuizPages",
                table: "QuizPages",
                column: "QuizPageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pages",
                table: "Pages",
                column: "PageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pages_Folders_FolderId",
                table: "Pages",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "FolderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pages_Lessons_LessonId",
                table: "Pages",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "LessonId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizOption_QuizPages_QuizPageId",
                table: "QuizOption",
                column: "QuizPageId",
                principalTable: "QuizPages",
                principalColumn: "QuizPageId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizPages_Quizzes_QuizId",
                table: "QuizPages",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "QuizId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

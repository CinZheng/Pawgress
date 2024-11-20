using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pawgress.Migrations
{
    /// <inheritdoc />
    public partial class MoreUpdatedMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDogProfiles_DogProfiles_DogProfileId1",
                table: "UserDogProfiles");

            migrationBuilder.DropTable(
                name: "Page");

            migrationBuilder.DropTable(
                name: "QuizOption");

            migrationBuilder.DropTable(
                name: "QuizPage");

            migrationBuilder.DropIndex(
                name: "IX_UserDogProfiles_DogProfileId1",
                table: "UserDogProfiles");

            migrationBuilder.DropColumn(
                name: "DogProfileId1",
                table: "UserDogProfiles");

            migrationBuilder.RenameColumn(
                name: "LessonName",
                table: "Lessons",
                newName: "Video");

            migrationBuilder.AddColumn<Guid>(
                name: "FolderId",
                table: "Quizzes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FolderId",
                table: "Lessons",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Lessons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "Lessons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Lessons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Tag",
                table: "Lessons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "Lessons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_FolderId",
                table: "Quizzes",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_FolderId",
                table: "Lessons",
                column: "FolderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Folders_FolderId",
                table: "Lessons",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "FolderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Folders_FolderId",
                table: "Quizzes",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "FolderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Folders_FolderId",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Folders_FolderId",
                table: "Quizzes");

            migrationBuilder.DropIndex(
                name: "IX_Quizzes_FolderId",
                table: "Quizzes");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_FolderId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "FolderId",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "FolderId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "Link",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "Tag",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "Lessons");

            migrationBuilder.RenameColumn(
                name: "Video",
                table: "Lessons",
                newName: "LessonName");

            migrationBuilder.AddColumn<Guid>(
                name: "DogProfileId1",
                table: "UserDogProfiles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Page",
                columns: table => new
                {
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LessonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Video = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Page", x => x.PageId);
                    table.ForeignKey(
                        name: "FK_Page_Folders_FolderId",
                        column: x => x.FolderId,
                        principalTable: "Folders",
                        principalColumn: "FolderId");
                    table.ForeignKey(
                        name: "FK_Page_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "LessonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizPage",
                columns: table => new
                {
                    QuizPageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuizId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizPage", x => x.QuizPageId);
                    table.ForeignKey(
                        name: "FK_QuizPage_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "QuizId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizOption",
                columns: table => new
                {
                    QuizOptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    OptionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuizPageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizOption", x => x.QuizOptionId);
                    table.ForeignKey(
                        name: "FK_QuizOption_QuizPage_QuizPageId",
                        column: x => x.QuizPageId,
                        principalTable: "QuizPage",
                        principalColumn: "QuizPageId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserDogProfiles_DogProfileId1",
                table: "UserDogProfiles",
                column: "DogProfileId1");

            migrationBuilder.CreateIndex(
                name: "IX_Page_FolderId",
                table: "Page",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Page_LessonId",
                table: "Page",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizOption_QuizPageId",
                table: "QuizOption",
                column: "QuizPageId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizPage_QuizId",
                table: "QuizPage",
                column: "QuizId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDogProfiles_DogProfiles_DogProfileId1",
                table: "UserDogProfiles",
                column: "DogProfileId1",
                principalTable: "DogProfiles",
                principalColumn: "DogProfileId");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pawgress.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DogProfiles",
                columns: table => new
                {
                    DogProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Breed = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DogProfiles", x => x.DogProfileId);
                });

            migrationBuilder.CreateTable(
                name: "Folders",
                columns: table => new
                {
                    FolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentFolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folders", x => x.FolderId);
                    table.ForeignKey(
                        name: "FK_Folders_Folders_ParentFolderId",
                        column: x => x.ParentFolderId,
                        principalTable: "Folders",
                        principalColumn: "FolderId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Libraries",
                columns: table => new
                {
                    LibraryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Libraries", x => x.LibraryId);
                });

            migrationBuilder.CreateTable(
                name: "TrainingPaths",
                columns: table => new
                {
                    TrainingPathId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingPaths", x => x.TrainingPathId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProgressData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Lessons",
                columns: table => new
                {
                    LessonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Video = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrainingPathId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LibraryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "Quizzes",
                columns: table => new
                {
                    QuizId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuizName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxScore = table.Column<int>(type: "int", nullable: true),
                    AchievedScore = table.Column<int>(type: "int", nullable: true),
                    TrainingPathId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LibraryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quizzes", x => x.QuizId);
                    table.ForeignKey(
                        name: "FK_Quizzes_Folders_FolderId",
                        column: x => x.FolderId,
                        principalTable: "Folders",
                        principalColumn: "FolderId");
                    table.ForeignKey(
                        name: "FK_Quizzes_Libraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Libraries",
                        principalColumn: "LibraryId");
                    table.ForeignKey(
                        name: "FK_Quizzes_TrainingPaths_TrainingPathId",
                        column: x => x.TrainingPathId,
                        principalTable: "TrainingPaths",
                        principalColumn: "TrainingPathId");
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    NoteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DogProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.NoteId);
                    table.ForeignKey(
                        name: "FK_Notes_DogProfiles_DogProfileId",
                        column: x => x.DogProfileId,
                        principalTable: "DogProfiles",
                        principalColumn: "DogProfileId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserDogProfiles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DogProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDogProfiles", x => new { x.UserId, x.DogProfileId });
                    table.ForeignKey(
                        name: "FK_UserDogProfiles_DogProfiles_DogProfileId",
                        column: x => x.DogProfileId,
                        principalTable: "DogProfiles",
                        principalColumn: "DogProfileId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDogProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTrainingPaths",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrainingPathId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Progress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTrainingPaths", x => new { x.UserId, x.TrainingPathId });
                    table.ForeignKey(
                        name: "FK_UserTrainingPaths_TrainingPaths_TrainingPathId",
                        column: x => x.TrainingPathId,
                        principalTable: "TrainingPaths",
                        principalColumn: "TrainingPathId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTrainingPaths_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Folders_ParentFolderId",
                table: "Folders",
                column: "ParentFolderId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Notes_DogProfileId",
                table: "Notes",
                column: "DogProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_UserId",
                table: "Notes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_FolderId",
                table: "Quizzes",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_LibraryId",
                table: "Quizzes",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_TrainingPathId",
                table: "Quizzes",
                column: "TrainingPathId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDogProfiles_DogProfileId",
                table: "UserDogProfiles",
                column: "DogProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTrainingPaths_TrainingPathId",
                table: "UserTrainingPaths",
                column: "TrainingPathId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lessons");

            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "Quizzes");

            migrationBuilder.DropTable(
                name: "UserDogProfiles");

            migrationBuilder.DropTable(
                name: "UserTrainingPaths");

            migrationBuilder.DropTable(
                name: "Folders");

            migrationBuilder.DropTable(
                name: "Libraries");

            migrationBuilder.DropTable(
                name: "DogProfiles");

            migrationBuilder.DropTable(
                name: "TrainingPaths");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

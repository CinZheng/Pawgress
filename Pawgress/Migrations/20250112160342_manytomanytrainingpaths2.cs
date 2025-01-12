using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pawgress.Migrations
{
    /// <inheritdoc />
    public partial class manytomanytrainingpaths2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TrainingPathItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "Unnamed Item");

            migrationBuilder.AddColumn<string>(
                name: "Lesson_Name",
                table: "TrainingPathItems",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lesson_Name",
                table: "TrainingPathItems");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TrainingPathItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Unnamed Item",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}

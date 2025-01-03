using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pawgress.Migrations
{
    /// <inheritdoc />
    public partial class AddSensorTypeEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DogSensorData_DogProfiles_DogProfileId",
                table: "DogSensorData");

            migrationBuilder.AlterColumn<int>(
                name: "SensorType",
                table: "DogSensorData",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "DogProfileId",
                table: "DogSensorData",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "AverageValue",
                table: "DogSensorData",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DogSensorData_DogProfiles_DogProfileId",
                table: "DogSensorData",
                column: "DogProfileId",
                principalTable: "DogProfiles",
                principalColumn: "DogProfileId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DogSensorData_DogProfiles_DogProfileId",
                table: "DogSensorData");

            migrationBuilder.AlterColumn<string>(
                name: "SensorType",
                table: "DogSensorData",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "DogProfileId",
                table: "DogSensorData",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<double>(
                name: "AverageValue",
                table: "DogSensorData",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddForeignKey(
                name: "FK_DogSensorData_DogProfiles_DogProfileId",
                table: "DogSensorData",
                column: "DogProfileId",
                principalTable: "DogProfiles",
                principalColumn: "DogProfileId");
        }
    }
}

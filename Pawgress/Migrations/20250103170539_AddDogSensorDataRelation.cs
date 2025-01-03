using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pawgress.Migrations
{
    /// <inheritdoc />
    public partial class AddDogSensorDataRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "AverageValue",
                table: "DogSensorData",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<Guid>(
                name: "DogProfileId",
                table: "DogSensorData",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DogSensorData_DogProfileId",
                table: "DogSensorData",
                column: "DogProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_DogSensorData_DogProfiles_DogProfileId",
                table: "DogSensorData",
                column: "DogProfileId",
                principalTable: "DogProfiles",
                principalColumn: "DogProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DogSensorData_DogProfiles_DogProfileId",
                table: "DogSensorData");

            migrationBuilder.DropIndex(
                name: "IX_DogSensorData_DogProfileId",
                table: "DogSensorData");

            migrationBuilder.DropColumn(
                name: "DogProfileId",
                table: "DogSensorData");

            migrationBuilder.AlterColumn<double>(
                name: "AverageValue",
                table: "DogSensorData",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);
        }
    }
}

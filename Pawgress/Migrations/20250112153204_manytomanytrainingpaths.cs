using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pawgress.Migrations
{
    /// <inheritdoc />
    public partial class manytomanytrainingpaths : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingPathItems_TrainingPaths_TrainingPathId",
                table: "TrainingPathItems");

            migrationBuilder.DropIndex(
                name: "IX_TrainingPathItems_TrainingPathId",
                table: "TrainingPathItems");

            migrationBuilder.CreateTable(
                name: "TrainingPathItemOrders",
                columns: table => new
                {
                    TrainingPathId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrainingPathItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingPathItemOrders", x => new { x.TrainingPathId, x.TrainingPathItemId });
                    table.ForeignKey(
                        name: "FK_TrainingPathItemOrders_TrainingPathItems_TrainingPathItemId",
                        column: x => x.TrainingPathItemId,
                        principalTable: "TrainingPathItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainingPathItemOrders_TrainingPaths_TrainingPathId",
                        column: x => x.TrainingPathId,
                        principalTable: "TrainingPaths",
                        principalColumn: "TrainingPathId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPathItemOrders_TrainingPathItemId",
                table: "TrainingPathItemOrders",
                column: "TrainingPathItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingPathItemOrders");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPathItems_TrainingPathId",
                table: "TrainingPathItems",
                column: "TrainingPathId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingPathItems_TrainingPaths_TrainingPathId",
                table: "TrainingPathItems",
                column: "TrainingPathId",
                principalTable: "TrainingPaths",
                principalColumn: "TrainingPathId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

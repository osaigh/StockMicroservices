using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StockMicroservices.API.Migrations
{
    public partial class Initialmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stock",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    Volume = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockHolder",
                columns: table => new
                {
                    Username = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockHolder", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "StockHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTimeOffset>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    StockId = table.Column<int>(nullable: false),
                    StockId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockHistory_Stock_StockId",
                        column: x => x.StockId,
                        principalTable: "Stock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockHistory_Stock_StockId1",
                        column: x => x.StockId1,
                        principalTable: "Stock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockPrice",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<double>(nullable: false),
                    Volume = table.Column<int>(nullable: false),
                    StockId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockPrice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockPrice_Stock_StockId",
                        column: x => x.StockId,
                        principalTable: "Stock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockHolderPosition",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CostBasis = table.Column<double>(nullable: false),
                    Shares = table.Column<int>(nullable: false),
                    StockId = table.Column<int>(nullable: false),
                    StockHolderId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockHolderPosition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockHolderPosition_StockHolder_StockHolderId",
                        column: x => x.StockHolderId,
                        principalTable: "StockHolder",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockHolderPosition_Stock_StockId",
                        column: x => x.StockId,
                        principalTable: "Stock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockHistory_StockId",
                table: "StockHistory",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_StockHistory_StockId1",
                table: "StockHistory",
                column: "StockId1");

            migrationBuilder.CreateIndex(
                name: "IX_StockHolderPosition_StockHolderId",
                table: "StockHolderPosition",
                column: "StockHolderId");

            migrationBuilder.CreateIndex(
                name: "IX_StockHolderPosition_StockId",
                table: "StockHolderPosition",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_StockPrice_StockId",
                table: "StockPrice",
                column: "StockId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockHistory");

            migrationBuilder.DropTable(
                name: "StockHolderPosition");

            migrationBuilder.DropTable(
                name: "StockPrice");

            migrationBuilder.DropTable(
                name: "StockHolder");

            migrationBuilder.DropTable(
                name: "Stock");
        }
    }
}

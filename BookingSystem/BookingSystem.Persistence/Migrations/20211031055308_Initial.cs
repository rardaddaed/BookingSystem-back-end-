using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookingSystem.Persistence.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookingLevel",
                columns: table => new
                {
                    BookingLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BlueprintUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BlueprintWidth = table.Column<int>(type: "int", nullable: true),
                    BlueprintHeight = table.Column<int>(type: "int", nullable: true),
                    MaxBooking = table.Column<int>(type: "int", nullable: false, defaultValue: 8),
                    Locked = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingLevel", x => x.BookingLevelId);
                });

            migrationBuilder.CreateTable(
                name: "BookingTeamArea",
                columns: table => new
                {
                    BookingTeamAreaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookingLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Coords = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Locked = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingTeamArea", x => x.BookingTeamAreaId);
                    table.ForeignKey(
                        name: "FK_BookingTeamArea_BookingLevel_BookingLevelId",
                        column: x => x.BookingLevelId,
                        principalTable: "BookingLevel",
                        principalColumn: "BookingLevelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingObject",
                columns: table => new
                {
                    BookingObjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookingObjectTypeId = table.Column<int>(type: "int", nullable: false),
                    BookingLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookingTeamAreaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Coords = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Locked = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingObject", x => x.BookingObjectId);
                    table.ForeignKey(
                        name: "FK_BookingObject_BookingLevel_BookingLevelId",
                        column: x => x.BookingLevelId,
                        principalTable: "BookingLevel",
                        principalColumn: "BookingLevelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingObject_BookingTeamArea_BookingTeamAreaId",
                        column: x => x.BookingTeamAreaId,
                        principalTable: "BookingTeamArea",
                        principalColumn: "BookingTeamAreaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingObject_BookingLevelId",
                table: "BookingObject",
                column: "BookingLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingObject_BookingTeamAreaId",
                table: "BookingObject",
                column: "BookingTeamAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingTeamArea_BookingLevelId",
                table: "BookingTeamArea",
                column: "BookingLevelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingObject");

            migrationBuilder.DropTable(
                name: "BookingTeamArea");

            migrationBuilder.DropTable(
                name: "BookingLevel");
        }
    }
}

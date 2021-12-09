using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingSystem.Persistence.Migrations
{
    public partial class AddBookingHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookingHistory",
                columns: table => new
                {
                    BookingHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookingObjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookedByAppNetUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CancledByAppNetUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BookingDateUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CancledDateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingHistory", x => x.BookingHistoryId);
                    table.ForeignKey(
                        name: "FK_BookingHistory_BookingObject_BookingObjectId",
                        column: x => x.BookingObjectId,
                        principalTable: "BookingObject",
                        principalColumn: "BookingObjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingHistory_BookingObjectId",
                table: "BookingHistory",
                column: "BookingObjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingHistory");
        }
    }
}

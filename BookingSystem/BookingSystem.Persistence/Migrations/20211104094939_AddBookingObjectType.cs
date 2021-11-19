using Microsoft.EntityFrameworkCore.Migrations;

namespace BookingSystem.Persistence.Migrations
{
    public partial class AddBookingObjectType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "BookingTeamArea");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "BookingObject");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "BookingLevel");

            migrationBuilder.CreateTable(
                name: "BookingObjectType",
                columns: table => new
                {
                    BookingObjectTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingObjectType", x => x.BookingObjectTypeId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingObjectType");

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "BookingTeamArea",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "BookingObject",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "BookingLevel",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

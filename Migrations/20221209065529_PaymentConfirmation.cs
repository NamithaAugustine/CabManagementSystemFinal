using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CabManagementSystems.Migrations
{
    /// <inheritdoc />
    public partial class PaymentConfirmation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Payed",
                table: "Bookings",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Payed",
                table: "Bookings");
        }
    }
}

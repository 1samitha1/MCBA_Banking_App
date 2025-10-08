using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerPortal.Migrations
{
    /// <inheritdoc />
    public partial class AddBillPayIsBlocked : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                table: "BillPay",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBlocked",
                table: "BillPay");
        }
    }
}

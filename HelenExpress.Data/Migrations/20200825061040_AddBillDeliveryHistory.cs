using HelenExpress.Data.JSONModels;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HelenExpress.Data.Migrations
{
    public partial class AddBillDeliveryHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<BillDeliveryHistory[]>(
                name: "billdeliveryhistories",
                table: "bill",
                type: "jsonb",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "billdeliveryhistories",
                table: "bill");
        }
    }
}

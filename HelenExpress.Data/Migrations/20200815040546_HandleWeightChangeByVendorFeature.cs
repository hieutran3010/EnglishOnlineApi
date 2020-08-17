using HelenExpress.Data.JSONModels;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HelenExpress.Data.Migrations
{
    public partial class HandleWeightChangeByVendorFeature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<PurchasePrice>(
                name: "oldpurchaseprice",
                table: "bill",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "vendorweightinkg",
                table: "bill",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "oldpurchaseprice",
                table: "bill");

            migrationBuilder.DropColumn(
                name: "vendorweightinkg",
                table: "bill");
        }
    }
}

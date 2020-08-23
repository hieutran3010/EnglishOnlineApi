using Microsoft.EntityFrameworkCore.Migrations;

namespace HelenExpress.Data.Migrations
{
    public partial class HandleManualInputPurchasePrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "oldpurchasepriceaftervatinusd",
                table: "bill",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "oldpurchasepriceaftervatinvnd",
                table: "bill",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "oldpurchasepriceinusd",
                table: "bill",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "oldpurchasepriceinvnd",
                table: "bill",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_bill_receivername",
                table: "bill",
                column: "receivername");

            migrationBuilder.CreateIndex(
                name: "ix_bill_sendername",
                table: "bill",
                column: "sendername");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_bill_receivername",
                table: "bill");

            migrationBuilder.DropIndex(
                name: "ix_bill_sendername",
                table: "bill");

            migrationBuilder.DropColumn(
                name: "oldpurchasepriceaftervatinusd",
                table: "bill");

            migrationBuilder.DropColumn(
                name: "oldpurchasepriceaftervatinvnd",
                table: "bill");

            migrationBuilder.DropColumn(
                name: "oldpurchasepriceinusd",
                table: "bill");

            migrationBuilder.DropColumn(
                name: "oldpurchasepriceinvnd",
                table: "bill");
        }
    }
}

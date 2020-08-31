using System.Text;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HelenExpress.Data.Migrations
{
    public partial class FixBugProfitCounting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "profitbeforetax",
                table: "bill");

            var sb = new StringBuilder();
            sb.Append("update bill ");
            sb.Append("set profit = COALESCE(customerpaymentamount, 0) - COALESCE(vendorpaymentamount, 0)");
            sb.Append("where status = 'DONE'");
            migrationBuilder.Sql(sb.ToString());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "profitbeforetax",
                table: "bill",
                type: "double precision",
                nullable: true);
        }
    }
}
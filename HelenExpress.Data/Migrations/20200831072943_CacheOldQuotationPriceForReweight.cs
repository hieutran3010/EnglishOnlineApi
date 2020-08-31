using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HelenExpress.Data.Migrations
{
    public partial class CacheOldQuotationPriceForReweight : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "lastupdatedquotation",
                table: "vendor",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "lastupdatedquotation",
                table: "bill",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "oldquotationpriceinusd",
                table: "bill",
                nullable: true);

            migrationBuilder.Sql(@"update vendor set lastupdatedquotation = modifiedon");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "lastupdatedquotation",
                table: "vendor");

            migrationBuilder.DropColumn(
                name: "lastupdatedquotation",
                table: "bill");

            migrationBuilder.DropColumn(
                name: "oldquotationpriceinusd",
                table: "bill");
        }
    }
}

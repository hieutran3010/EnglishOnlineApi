using System.Text;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HelenExpress.Data.Migrations
{
    public partial class RemovePackageStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var migrationPackageStatusSqlSb = new StringBuilder();
            migrationPackageStatusSqlSb.Append("update bill ");
            migrationPackageStatusSqlSb.Append(
                @"set billdeliveryhistories = FORMAT('[{""Date"": null, ""Time"": null, ""Status"": ""%s""}]', packagestatus)::jsonb ");
            migrationPackageStatusSqlSb.Append(@"where packagestatus is not null or trim(packagestatus) = ''");
            migrationBuilder.Sql(migrationPackageStatusSqlSb.ToString());

            migrationBuilder.DropColumn(
                name: "packagestatus",
                table: "bill");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "packagestatus",
                table: "bill",
                type: "citext",
                nullable: true);
        }
    }
}
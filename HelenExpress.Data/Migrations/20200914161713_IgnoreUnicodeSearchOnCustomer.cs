using Microsoft.EntityFrameworkCore.Migrations;

namespace HelenExpress.Data.Migrations
{
    public partial class IgnoreUnicodeSearchOnCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "namenonunicode",
                table: "customer",
                type: "citext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "receivernamenonunicode",
                table: "bill",
                type: "citext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "sendernamenonunicode",
                table: "bill",
                type: "citext",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_customer_namenonunicode",
                table: "customer",
                column: "namenonunicode");

            migrationBuilder.CreateIndex(
                name: "ix_bill_receivernamenonunicode",
                table: "bill",
                column: "receivernamenonunicode");

            migrationBuilder.CreateIndex(
                name: "ix_bill_sendernamenonunicode",
                table: "bill",
                column: "sendernamenonunicode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_customer_namenonunicode",
                table: "customer");

            migrationBuilder.DropIndex(
                name: "ix_bill_receivernamenonunicode",
                table: "bill");

            migrationBuilder.DropIndex(
                name: "ix_bill_sendernamenonunicode",
                table: "bill");

            migrationBuilder.DropColumn(
                name: "namenonunicode",
                table: "customer");

            migrationBuilder.DropColumn(
                name: "receivernamenonunicode",
                table: "bill");

            migrationBuilder.DropColumn(
                name: "sendernamenonunicode",
                table: "bill");
        }
    }
}

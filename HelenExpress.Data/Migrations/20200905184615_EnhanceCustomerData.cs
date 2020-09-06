using Microsoft.EntityFrameworkCore.Migrations;

namespace HelenExpress.Data.Migrations
{
    public partial class EnhanceCustomerData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_customer_phone",
                table: "customer");

            migrationBuilder.DropColumn(
                name: "nickname",
                table: "customer");

            migrationBuilder.DropColumn(
                name: "saleuserid",
                table: "customer");

            migrationBuilder.CreateIndex(
                name: "ix_customer_phone",
                table: "customer",
                column: "phone",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_customer_phone",
                table: "customer");

            migrationBuilder.AddColumn<string>(
                name: "nickname",
                table: "customer",
                type: "citext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "saleuserid",
                table: "customer",
                type: "citext",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_customer_phone",
                table: "customer",
                column: "phone");
        }
    }
}

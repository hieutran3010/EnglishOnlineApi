using Microsoft.EntityFrameworkCore.Migrations;

namespace HelenExpress.Data.Migrations
{
    public partial class RemoveConstraintUniqueIndexOnBillIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_bill_airlinebillid",
                table: "bill");

            migrationBuilder.DropIndex(
                name: "ix_bill_childbillid",
                table: "bill");

            migrationBuilder.CreateIndex(
                name: "ix_bill_airlinebillid",
                table: "bill",
                column: "airlinebillid");

            migrationBuilder.CreateIndex(
                name: "ix_bill_childbillid",
                table: "bill",
                column: "childbillid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_bill_airlinebillid",
                table: "bill");

            migrationBuilder.DropIndex(
                name: "ix_bill_childbillid",
                table: "bill");

            migrationBuilder.CreateIndex(
                name: "ix_bill_airlinebillid",
                table: "bill",
                column: "airlinebillid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_bill_childbillid",
                table: "bill",
                column: "childbillid",
                unique: true);
        }
    }
}

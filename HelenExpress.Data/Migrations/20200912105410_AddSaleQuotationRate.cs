using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HelenExpress.Data.Migrations
{
    public partial class AddSaleQuotationRate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_bill_vendors_vendorid",
                table: "bill");

            migrationBuilder.CreateTable(
                name: "salequotationrate",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false, defaultValueSql: "uuid_generate_v1mc()"),
                    createdon = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "timezone('utc'::text, now())"),
                    createdby = table.Column<string>(type: "citext", nullable: false),
                    modifiedon = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "timezone('utc'::text, now())"),
                    modifiedby = table.Column<string>(type: "citext", nullable: false),
                    fromweight = table.Column<double>(nullable: false),
                    toweight = table.Column<double>(nullable: true),
                    percent = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_salequotationrate", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_salequotationrate_toweight",
                table: "salequotationrate",
                column: "toweight");

            migrationBuilder.CreateIndex(
                name: "ix_salequotationrate_fromweight_toweight",
                table: "salequotationrate",
                columns: new[] { "fromweight", "toweight" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_bill_vendors_vendorid",
                table: "bill",
                column: "vendorid",
                principalTable: "vendor",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_bill_vendors_vendorid",
                table: "bill");

            migrationBuilder.DropTable(
                name: "salequotationrate");

            migrationBuilder.AddForeignKey(
                name: "fk_bill_vendors_vendorid",
                table: "bill",
                column: "vendorid",
                principalTable: "vendor",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

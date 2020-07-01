using System;
using HelenExpress.Data.JSONModels;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HelenExpress.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:citext", ",,")
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "billdescription",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false, defaultValueSql: "uuid_generate_v1mc()"),
                    createdon = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "timezone('utc'::text, now())"),
                    createdby = table.Column<string>(type: "citext", nullable: false),
                    modifiedon = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "timezone('utc'::text, now())"),
                    modifiedby = table.Column<string>(type: "citext", nullable: false),
                    name = table.Column<string>(type: "citext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_billdescription", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "customer",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false, defaultValueSql: "uuid_generate_v1mc()"),
                    createdon = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "timezone('utc'::text, now())"),
                    createdby = table.Column<string>(type: "citext", nullable: false),
                    modifiedon = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "timezone('utc'::text, now())"),
                    modifiedby = table.Column<string>(type: "citext", nullable: false),
                    code = table.Column<string>(type: "citext", nullable: true),
                    name = table.Column<string>(type: "citext", nullable: true),
                    nickname = table.Column<string>(type: "citext", nullable: true),
                    phone = table.Column<string>(type: "citext", nullable: true),
                    address = table.Column<string>(type: "citext", nullable: true),
                    hint = table.Column<string>(type: "citext", nullable: true),
                    saleuserid = table.Column<string>(type: "citext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customer", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "exportsession",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false, defaultValueSql: "uuid_generate_v1mc()"),
                    createdon = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "timezone('utc'::text, now())"),
                    createdby = table.Column<string>(type: "citext", nullable: false),
                    modifiedon = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "timezone('utc'::text, now())"),
                    modifiedby = table.Column<string>(type: "citext", nullable: false),
                    userid = table.Column<string>(type: "citext", nullable: true),
                    status = table.Column<string>(type: "citext", nullable: true),
                    filepath = table.Column<string>(type: "citext", nullable: true),
                    exporttype = table.Column<string>(type: "citext", nullable: true),
                    note = table.Column<string>(type: "citext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_exportsession", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "params",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false, defaultValueSql: "uuid_generate_v1mc()"),
                    createdon = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "timezone('utc'::text, now())"),
                    createdby = table.Column<string>(type: "citext", nullable: false),
                    modifiedon = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "timezone('utc'::text, now())"),
                    modifiedby = table.Column<string>(type: "citext", nullable: false),
                    key = table.Column<string>(type: "citext", nullable: true),
                    value = table.Column<string>(type: "citext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_params", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "userrole",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false, defaultValueSql: "uuid_generate_v1mc()"),
                    createdon = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "timezone('utc'::text, now())"),
                    createdby = table.Column<string>(type: "citext", nullable: false),
                    modifiedon = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "timezone('utc'::text, now())"),
                    modifiedby = table.Column<string>(type: "citext", nullable: false),
                    userid = table.Column<string>(type: "citext", nullable: true),
                    role = table.Column<string>(type: "citext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_userrole", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "vendor",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false, defaultValueSql: "uuid_generate_v1mc()"),
                    createdon = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "timezone('utc'::text, now())"),
                    createdby = table.Column<string>(type: "citext", nullable: false),
                    modifiedon = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "timezone('utc'::text, now())"),
                    modifiedby = table.Column<string>(type: "citext", nullable: false),
                    name = table.Column<string>(type: "citext", nullable: true),
                    officeaddress = table.Column<string>(type: "citext", nullable: true),
                    phone = table.Column<string>(type: "citext", nullable: true),
                    otherfeeinusd = table.Column<double>(nullable: true),
                    fuelchargepercent = table.Column<double>(nullable: true),
                    isstopped = table.Column<bool>(nullable: false),
                    vendorquotations = table.Column<VendorQuotation[]>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vendor", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "bill",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false, defaultValueSql: "uuid_generate_v1mc()"),
                    createdon = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "timezone('utc'::text, now())"),
                    createdby = table.Column<string>(type: "citext", nullable: false),
                    modifiedon = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "timezone('utc'::text, now())"),
                    modifiedby = table.Column<string>(type: "citext", nullable: false),
                    saleuserid = table.Column<string>(type: "citext", nullable: true),
                    licenseuserid = table.Column<string>(type: "citext", nullable: true),
                    accountantuserid = table.Column<string>(type: "citext", nullable: true),
                    sendername = table.Column<string>(type: "citext", nullable: true),
                    senderphone = table.Column<string>(type: "citext", nullable: true),
                    senderaddress = table.Column<string>(type: "citext", nullable: true),
                    receivername = table.Column<string>(type: "citext", nullable: true),
                    receiverphone = table.Column<string>(type: "citext", nullable: true),
                    receiveraddress = table.Column<string>(type: "citext", nullable: true),
                    date = table.Column<DateTime>(nullable: false),
                    period = table.Column<string>(type: "citext", nullable: true),
                    childbillid = table.Column<string>(type: "citext", nullable: true),
                    airlinebillid = table.Column<string>(type: "citext", nullable: true),
                    vendorid = table.Column<Guid>(nullable: false),
                    vendorname = table.Column<string>(type: "citext", nullable: true),
                    senderid = table.Column<Guid>(nullable: true),
                    receiverid = table.Column<Guid>(nullable: true),
                    internationalparcelvendor = table.Column<string>(type: "citext", nullable: true),
                    description = table.Column<string>(type: "citext", nullable: true),
                    destinationcountry = table.Column<string>(type: "citext", nullable: true),
                    weightinkg = table.Column<double>(nullable: false),
                    saleprice = table.Column<double>(nullable: true),
                    purchasepriceinusd = table.Column<double>(nullable: true),
                    purchasepriceinvnd = table.Column<int>(nullable: true),
                    purchasepriceaftervatinusd = table.Column<double>(nullable: true),
                    purchasepriceaftervatinvnd = table.Column<int>(nullable: true),
                    profit = table.Column<double>(nullable: true),
                    profitbeforetax = table.Column<double>(nullable: true),
                    vat = table.Column<long>(nullable: true),
                    status = table.Column<string>(type: "citext", nullable: true),
                    vendornetpriceinusd = table.Column<double>(nullable: true),
                    vendorotherfee = table.Column<double>(nullable: false),
                    vendorfuelchargepercent = table.Column<double>(nullable: false),
                    vendorfuelchargefeeinusd = table.Column<double>(nullable: true),
                    vendorfuelchargefeeinvnd = table.Column<double>(nullable: true),
                    customerpaymenttype = table.Column<string>(type: "citext", nullable: true),
                    customerpaymentamount = table.Column<double>(nullable: true),
                    customerpaymentdebt = table.Column<double>(nullable: true),
                    vendorpaymenttype = table.Column<string>(type: "citext", nullable: true),
                    vendorpaymentamount = table.Column<double>(nullable: true),
                    vendorpaymentdebt = table.Column<double>(nullable: true),
                    isarchived = table.Column<bool>(nullable: false),
                    usdexchangerate = table.Column<int>(nullable: true),
                    quotationpriceinusd = table.Column<double>(nullable: true),
                    zonename = table.Column<string>(type: "citext", nullable: true),
                    isprintedvatbill = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bill", x => x.id);
                    table.ForeignKey(
                        name: "fk_bill_customers_receiverid",
                        column: x => x.receiverid,
                        principalTable: "customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_bill_customers_senderid",
                        column: x => x.senderid,
                        principalTable: "customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_bill_vendors_vendorid",
                        column: x => x.vendorid,
                        principalTable: "vendor",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "zone",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false, defaultValueSql: "uuid_generate_v1mc()"),
                    createdon = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "timezone('utc'::text, now())"),
                    createdby = table.Column<string>(type: "citext", nullable: false),
                    modifiedon = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "timezone('utc'::text, now())"),
                    modifiedby = table.Column<string>(type: "citext", nullable: false),
                    name = table.Column<string>(type: "citext", nullable: true),
                    countries = table.Column<string[]>(type: "jsonb", nullable: true),
                    vendorid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_zone", x => x.id);
                    table.ForeignKey(
                        name: "fk_zone_vendor_vendorid",
                        column: x => x.vendorid,
                        principalTable: "vendor",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_bill_airlinebillid",
                table: "bill",
                column: "airlinebillid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_bill_date",
                table: "bill",
                column: "date");

            migrationBuilder.CreateIndex(
                name: "ix_bill_period",
                table: "bill",
                column: "period");

            migrationBuilder.CreateIndex(
                name: "ix_bill_receiverid",
                table: "bill",
                column: "receiverid");

            migrationBuilder.CreateIndex(
                name: "ix_bill_senderid",
                table: "bill",
                column: "senderid");

            migrationBuilder.CreateIndex(
                name: "ix_bill_vendorid",
                table: "bill",
                column: "vendorid");

            migrationBuilder.CreateIndex(
                name: "ix_billdescription_name",
                table: "billdescription",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_customer_address",
                table: "customer",
                column: "address");

            migrationBuilder.CreateIndex(
                name: "ix_customer_code",
                table: "customer",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_customer_name",
                table: "customer",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_customer_phone",
                table: "customer",
                column: "phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_exportsession_userid_exporttype",
                table: "exportsession",
                columns: new[] { "userid", "exporttype" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_userrole_role",
                table: "userrole",
                column: "role");

            migrationBuilder.CreateIndex(
                name: "ix_userrole_userid_role",
                table: "userrole",
                columns: new[] { "userid", "role" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_vendor_name",
                table: "vendor",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_zone_vendorid",
                table: "zone",
                column: "vendorid");

            migrationBuilder.CreateIndex(
                name: "ix_zone_name_vendorid",
                table: "zone",
                columns: new[] { "name", "vendorid" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bill");

            migrationBuilder.DropTable(
                name: "billdescription");

            migrationBuilder.DropTable(
                name: "exportsession");

            migrationBuilder.DropTable(
                name: "params");

            migrationBuilder.DropTable(
                name: "userrole");

            migrationBuilder.DropTable(
                name: "zone");

            migrationBuilder.DropTable(
                name: "customer");

            migrationBuilder.DropTable(
                name: "vendor");
        }
    }
}

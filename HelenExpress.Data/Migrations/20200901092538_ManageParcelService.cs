using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HelenExpress.Data.Migrations
{
    public partial class ManageParcelService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "parcelservicezoneid",
                table: "zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "parcelservice",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false, defaultValueSql: "uuid_generate_v1mc()"),
                    createdon = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "timezone('utc'::text, now())"),
                    createdby = table.Column<string>(type: "citext", nullable: false),
                    modifiedon = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "timezone('utc'::text, now())"),
                    modifiedby = table.Column<string>(type: "citext", nullable: false),
                    name = table.Column<string>(type: "citext", nullable: true),
                    issystem = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_parcelservice", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "parcelservicezone",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false, defaultValueSql: "uuid_generate_v1mc()"),
                    createdon = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "timezone('utc'::text, now())"),
                    createdby = table.Column<string>(type: "citext", nullable: false),
                    modifiedon = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "timezone('utc'::text, now())"),
                    modifiedby = table.Column<string>(type: "citext", nullable: false),
                    name = table.Column<string>(type: "citext", nullable: true),
                    countries = table.Column<string[]>(type: "jsonb", nullable: true),
                    parcelserviceid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_parcelservicezone", x => x.id);
                    table.ForeignKey(
                        name: "fk_parcelservicezone_parcelservice_parcelserviceid",
                        column: x => x.parcelserviceid,
                        principalTable: "parcelservice",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_zone_parcelservicezoneid",
                table: "zone",
                column: "parcelservicezoneid");

            migrationBuilder.CreateIndex(
                name: "ix_parcelservice_name",
                table: "parcelservice",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_parcelservicezone_parcelserviceid",
                table: "parcelservicezone",
                column: "parcelserviceid");

            migrationBuilder.CreateIndex(
                name: "ix_parcelservicezone_name_parcelserviceid",
                table: "parcelservicezone",
                columns: new[] { "name", "parcelserviceid" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_zone_parcelservicezone_parcelservicezoneid",
                table: "zone",
                column: "parcelservicezoneid",
                principalTable: "parcelservicezone",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
            
            migrationBuilder.Sql(@"insert into parcelservice(createdon, createdby, modifiedon, modifiedby, name, issystem)
                                    values(NOW(), 'System', NOW(), 'System', 'DHL VN', true);
                                    insert into parcelservice(createdon, createdby, modifiedon, modifiedby, name, issystem)
                                    values(NOW(), 'System', NOW(), 'System', 'DHL Sing', true);
                                    insert into parcelservice(createdon, createdby, modifiedon, modifiedby, name, issystem)
                                    values(NOW(), 'System', NOW(), 'System', 'UPS', true);
                                    insert into parcelservice(createdon, createdby, modifiedon, modifiedby, name, issystem)
                                    values(NOW(), 'System', NOW(), 'System', 'TNT', true);
                                    insert into parcelservice(createdon, createdby, modifiedon, modifiedby, name, issystem)
                                    values(NOW(), 'System', NOW(), 'System', 'Fedex', true);");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_zone_parcelservicezone_parcelservicezoneid",
                table: "zone");

            migrationBuilder.DropTable(
                name: "parcelservicezone");

            migrationBuilder.DropTable(
                name: "parcelservice");

            migrationBuilder.DropIndex(
                name: "ix_zone_parcelservicezoneid",
                table: "zone");

            migrationBuilder.DropColumn(
                name: "parcelservicezoneid",
                table: "zone");
        }
    }
}

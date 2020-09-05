using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HelenExpress.Data.Migrations
{
    public partial class AddParcelServiceVendorAssociation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_zone_parcelservicezone_parcelservicezoneid",
                table: "zone");

            migrationBuilder.DropIndex(
                name: "ix_zone_parcelservicezoneid",
                table: "zone");

            migrationBuilder.DropColumn(
                name: "parcelservicezoneid",
                table: "zone");

            migrationBuilder.CreateTable(
                name: "parcelservicevendor",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false, defaultValueSql: "uuid_generate_v1mc()"),
                    createdon = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "timezone('utc'::text, now())"),
                    createdby = table.Column<string>(type: "citext", nullable: false),
                    modifiedon = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "timezone('utc'::text, now())"),
                    modifiedby = table.Column<string>(type: "citext", nullable: false),
                    parcelserviceid = table.Column<Guid>(nullable: false),
                    vendorid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_parcelservicevendor", x => x.id);
                    table.ForeignKey(
                        name: "fk_parcelservicevendor_parcelservice_parcelserviceid",
                        column: x => x.parcelserviceid,
                        principalTable: "parcelservice",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_parcelservicevendor_vendors_vendorid",
                        column: x => x.vendorid,
                        principalTable: "vendor",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_parcelservicevendor_vendorid",
                table: "parcelservicevendor",
                column: "vendorid");

            migrationBuilder.CreateIndex(
                name: "ix_parcelservicevendor_parcelserviceid_vendorid",
                table: "parcelservicevendor",
                columns: new[] { "parcelserviceid", "vendorid" },
                unique: true);
            
            // trigger to update service name for on vendor zone
            migrationBuilder.Sql(@"CREATE OR REPLACE FUNCTION tg_fn_sync_serviceName_vendorZones() 
                                        RETURNS TRIGGER
                                        LANGUAGE plpgsql
                                        AS
                                    $parcelservice_table$
                                    BEGIN
					                    IF (NEW.name <> OLD.name) THEN
						                    UPDATE zone
						                    SET name = REPLACE(name, OLD.name, NEW.name)
						                    WHERE name like CONCAT(OLD.name, '-', '%');
					                    END IF;
                                        RETURN NULL;
                                    END;
                                    $parcelservice_table$");
            migrationBuilder.Sql(@"CREATE TRIGGER tg_sync_serviceName_vendorZones
                                    AFTER UPDATE OF name
                                    ON parcelservice
                                    FOR EACH ROW EXECUTE PROCEDURE tg_fn_sync_serviceName_vendorZones();");
            // trigger to remove all related vendor zones when deleting a service
            migrationBuilder.Sql(@"CREATE OR REPLACE FUNCTION tg_fn_sync_serviceDeleted_vendorZones() 
				                    RETURNS TRIGGER
				                    LANGUAGE plpgsql
				                    AS
				                $$
				                BEGIN
									DELETE FROM zone
									WHERE name LIKE CONCAT(OLD.name, '-', '%');
								RETURN NULL;
								END;
				                $$");
            migrationBuilder.Sql(@"CREATE TRIGGER tg_sync_serviceDeleted_vendorZones
									AFTER DELETE ON parcelservice
									FOR EACH ROW EXECUTE PROCEDURE tg_fn_sync_serviceDeleted_vendorZones();");
            
            // trigger to update service zone name, service zone countries to vendor zones when updating
            migrationBuilder.Sql(@"CREATE OR REPLACE FUNCTION tg_fn_sync_serviceZone_vendorZones() 
                                        RETURNS TRIGGER
                                        LANGUAGE plpgsql
                                        AS
                                    $parcelservicezone_table$
				                    DECLARE
					                    serviceName citext;
                                    BEGIN
					                    SELECT name INTO serviceName
					                    FROM parcelservice
					                    WHERE id = OLD.parcelserviceid;
					                    
					                    UPDATE zone
					                    SET name = REPLACE(name, OLD.name, NEW.name), countries = NEW.countries
					                    WHERE name like CONCAT(serviceName, '-', '%');
                                        RETURN NULL;
                                    END;
                                    $parcelservicezone_table$");
            migrationBuilder.Sql(@"CREATE TRIGGER tg_sync_serviceZone_vendorZones
                                    AFTER UPDATE ON parcelservicezone
                                    FOR EACH ROW EXECUTE PROCEDURE tg_fn_sync_serviceZone_vendorZones();");
            
            // when insert new zone to a service that existed a mapping, let auto insert to all mapping
            migrationBuilder.Sql(@"CREATE OR REPLACE FUNCTION tg_fn_sync_serviceZoneInsert_vendorZones() 
                                    RETURNS TRIGGER
                                    LANGUAGE plpgsql
                                    AS
                                $$
				                DECLARE
					                serviceName citext;
					                servicevendorassociation record;
                                BEGIN
					                SELECT name INTO servicename
					                FROM parcelservice
					                WHERE id = NEW.parcelserviceid;
					                
					                FOR servicevendorassociation IN SELECT vendorid
									   				                FROM parcelservicevendor
									   				                WHERE parcelserviceid = NEW.parcelserviceid
					                LOOP                                               
        				                INSERT INTO zone (name, countries, vendorid, createdon, createdby, modifiedon, modifiedby)
						                VALUES(CONCAT(serviceName, '-', NEW.name), NEW.countries, servicevendorassociation.vendorid, NOW(), 'DB Trigger', NOW(), 'DB Trigger');
    				                END LOOP;
				                RETURN NULL;
				                END;
                                $$");
            migrationBuilder.Sql(@"CREATE TRIGGER tg_sync_serviceZoneInsert_vendorZones
                                    AFTER INSERT ON parcelservicezone
                                    FOR EACH ROW EXECUTE PROCEDURE tg_fn_sync_serviceZoneInsert_vendorZones();");
            
            // when insert a zone from a service that existed a mapping, let auto delete to all mapping
            migrationBuilder.Sql(@"CREATE OR REPLACE FUNCTION tg_fn_sync_serviceZoneDeleted_vendorZones() 
                                    RETURNS TRIGGER
                                    LANGUAGE plpgsql
                                    AS
                                $parcelservicezone_table$
				                DECLARE
					                serviceName citext;
                                BEGIN
					                SELECT name INTO serviceName
					                FROM parcelservice
					                WHERE id = OLD.parcelserviceid;
					                
					                DELETE FROM zone
					                WHERE name = CONCAT(serviceName, '-', OLD.name);
                                    RETURN NULL;
                                END;
                                $parcelservicezone_table$");
            migrationBuilder.Sql(@"CREATE TRIGGER tg_sync_serviceZoneDeleted_vendorZones
                                    AFTER DELETE ON parcelservicezone
                                    FOR EACH ROW EXECUTE PROCEDURE tg_fn_sync_serviceZoneDeleted_vendorZones();");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "parcelservicevendor");

            migrationBuilder.AddColumn<Guid>(
                name: "parcelservicezoneid",
                table: "zone",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_zone_parcelservicezoneid",
                table: "zone",
                column: "parcelservicezoneid");

            migrationBuilder.AddForeignKey(
                name: "fk_zone_parcelservicezone_parcelservicezoneid",
                table: "zone",
                column: "parcelservicezoneid",
                principalTable: "parcelservicezone",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS tg_fn_sync_serviceName_vendorZones CASCADE;");
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS tg_fn_sync_serviceZone_vendorZones CASCADE;");
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS tg_fn_sync_serviceZoneInsert_vendorZones CASCADE;");
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS tg_fn_sync_serviceZoneDeleted_vendorZones CASCADE;");
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS tg_fn_sync_serviceDeleted_vendorZones CASCADE;");
        }
    }
}

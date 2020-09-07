using Microsoft.EntityFrameworkCore.Migrations;

namespace HelenExpress.Data.Migrations
{
    public partial class AddBillTriggerOnVendorName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR REPLACE FUNCTION tg_fn_sync_bill_vendorName() 
                    RETURNS TRIGGER
                    LANGUAGE plpgsql
                    AS
                $bill_table$
                BEGIN
					IF (TG_OP = 'INSERT' OR (TG_OP = 'UPDATE' AND NEW.vendorid <> OLD.vendorid)) THEN
						UPDATE bill
						SET vendorname = vendor.name
						FROM (SELECT name, id 
							  FROM vendor 
							  WHERE id = NEW.vendorid) AS vendor
						WHERE bill.id = NEW.id;
					END IF;
                    RETURN NULL;
                END;
                $bill_table$");

            migrationBuilder.Sql(@"DROP TRIGGER IF EXISTS tg_sync_bill_vendorName ON bill;
				CREATE TRIGGER tg_sync_bill_vendorName
                AFTER INSERT OR UPDATE OF vendorid
                ON bill
                    FOR EACH ROW EXECUTE PROCEDURE tg_fn_sync_bill_vendorName();");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS tg_fn_sync_bill_vendorName CASCADE;");
        }
    }
}

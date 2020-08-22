using Microsoft.EntityFrameworkCore.Migrations;

namespace HelenExpress.Data.Migrations
{
    public partial class HandleWeightAndOldWeight : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "oldweightinkg",
                table: "bill",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "oldweightinkg",
                table: "bill");
        }
    }
}

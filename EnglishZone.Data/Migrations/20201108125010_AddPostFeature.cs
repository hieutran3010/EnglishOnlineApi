using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EnglishZone.Data.Migrations
{
    public partial class AddPostFeature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:citext", ",,")
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "post",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false, defaultValueSql: "uuid_generate_v1mc()"),
                    createdon = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "timezone('utc'::text, now())"),
                    createdby = table.Column<string>(type: "citext", nullable: false),
                    modifiedon = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "timezone('utc'::text, now())"),
                    modifiedby = table.Column<string>(type: "citext", nullable: false),
                    owner = table.Column<string>(type: "citext", nullable: true),
                    content = table.Column<string>(type: "citext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_post", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "postcomment",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false, defaultValueSql: "uuid_generate_v1mc()"),
                    createdon = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "timezone('utc'::text, now())"),
                    createdby = table.Column<string>(type: "citext", nullable: false),
                    modifiedon = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "timezone('utc'::text, now())"),
                    modifiedby = table.Column<string>(type: "citext", nullable: false),
                    owner = table.Column<string>(type: "citext", nullable: true),
                    content = table.Column<string>(type: "citext", nullable: true),
                    postid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_postcomment", x => x.id);
                    table.ForeignKey(
                        name: "fk_postcomment_post_postid",
                        column: x => x.postid,
                        principalTable: "post",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_post_owner",
                table: "post",
                column: "owner");

            migrationBuilder.CreateIndex(
                name: "ix_postcomment_owner",
                table: "postcomment",
                column: "owner");

            migrationBuilder.CreateIndex(
                name: "ix_postcomment_postid",
                table: "postcomment",
                column: "postid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "postcomment");

            migrationBuilder.DropTable(
                name: "post");
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentVsUniversity.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Acivities",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Color = table.Column<string>(nullable: true),
                    EllapsedMinutes = table.Column<int>(nullable: false),
                    EllapsedRestTime = table.Column<int>(nullable: false),
                    EllapsedWorkTime = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    RestTime = table.Column<int>(nullable: false),
                    WorkTime = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Acivities", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Acivities");
        }
    }
}

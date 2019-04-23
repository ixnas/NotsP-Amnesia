using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Amnesia.Domain.Migrations
{
    public partial class Addstate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "State",
                columns: table => new
                {
                    CurrentBlockHash = table.Column<byte[]>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_State", x => x.CurrentBlockHash);
                    table.ForeignKey(
                        name: "FK_State_Blocks_CurrentBlockHash",
                        column: x => x.CurrentBlockHash,
                        principalTable: "Blocks",
                        principalColumn: "Hash",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "State");
        }
    }
}

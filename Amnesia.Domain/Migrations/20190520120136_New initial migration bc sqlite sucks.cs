using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Amnesia.Domain.Migrations
{
    public partial class Newinitialmigrationbcsqlitesucks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contents",
                columns: table => new
                {
                    Hash = table.Column<byte[]>(nullable: false),
                    Definitions = table.Column<string>(nullable: true),
                    Mutations = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contents", x => x.Hash);
                });

            migrationBuilder.CreateTable(
                name: "Blocks",
                columns: table => new
                {
                    Hash = table.Column<byte[]>(nullable: false),
                    PreviousBlockHash = table.Column<byte[]>(nullable: true),
                    Nonce = table.Column<int>(nullable: false),
                    ContentHash = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blocks", x => x.Hash);
                    table.ForeignKey(
                        name: "FK_Blocks_Contents_ContentHash",
                        column: x => x.ContentHash,
                        principalTable: "Contents",
                        principalColumn: "Hash",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Blocks_Blocks_PreviousBlockHash",
                        column: x => x.PreviousBlockHash,
                        principalTable: "Blocks",
                        principalColumn: "Hash",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "State",
                columns: table => new
                {
                    PeerId = table.Column<string>(nullable: false),
                    CurrentBlockHash = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_State", x => x.PeerId);
                    table.ForeignKey(
                        name: "FK_State_Blocks_CurrentBlockHash",
                        column: x => x.CurrentBlockHash,
                        principalTable: "Blocks",
                        principalColumn: "Hash",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Definitions",
                columns: table => new
                {
                    Hash = table.Column<byte[]>(nullable: false),
                    DataHash = table.Column<byte[]>(nullable: true),
                    PreviousDefinitionHash = table.Column<byte[]>(nullable: true),
                    Signature = table.Column<byte[]>(nullable: true),
                    Key = table.Column<byte[]>(nullable: true),
                    IsMutation = table.Column<bool>(nullable: false),
                    Meta = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Definitions", x => x.Hash);
                    table.ForeignKey(
                        name: "FK_Definitions_Definitions_PreviousDefinitionHash",
                        column: x => x.PreviousDefinitionHash,
                        principalTable: "Definitions",
                        principalColumn: "Hash",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Data",
                columns: table => new
                {
                    Hash = table.Column<byte[]>(nullable: false),
                    PreviousDefinitionHash = table.Column<byte[]>(nullable: true),
                    Signature = table.Column<byte[]>(nullable: true),
                    Key = table.Column<byte[]>(nullable: true),
                    Blob = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Data", x => x.Hash);
                    table.ForeignKey(
                        name: "FK_Data_Definitions_PreviousDefinitionHash",
                        column: x => x.PreviousDefinitionHash,
                        principalTable: "Definitions",
                        principalColumn: "Hash",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blocks_ContentHash",
                table: "Blocks",
                column: "ContentHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Blocks_PreviousBlockHash",
                table: "Blocks",
                column: "PreviousBlockHash");

            migrationBuilder.CreateIndex(
                name: "IX_Data_PreviousDefinitionHash",
                table: "Data",
                column: "PreviousDefinitionHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Definitions_DataHash",
                table: "Definitions",
                column: "DataHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Definitions_PreviousDefinitionHash",
                table: "Definitions",
                column: "PreviousDefinitionHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_State_CurrentBlockHash",
                table: "State",
                column: "CurrentBlockHash");

            migrationBuilder.AddForeignKey(
                name: "FK_Definitions_Data_DataHash",
                table: "Definitions",
                column: "DataHash",
                principalTable: "Data",
                principalColumn: "Hash",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Data_Definitions_PreviousDefinitionHash",
                table: "Data");

            migrationBuilder.DropTable(
                name: "State");

            migrationBuilder.DropTable(
                name: "Blocks");

            migrationBuilder.DropTable(
                name: "Contents");

            migrationBuilder.DropTable(
                name: "Definitions");

            migrationBuilder.DropTable(
                name: "Data");
        }
    }
}

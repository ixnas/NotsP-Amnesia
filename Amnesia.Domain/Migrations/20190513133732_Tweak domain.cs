using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Amnesia.Domain.Migrations
{
    public partial class Tweakdomain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Definitions_Contents_ContentDefinitionHash",
                table: "Definitions");

            migrationBuilder.DropForeignKey(
                name: "FK_Definitions_Contents_ContentMutationHash",
                table: "Definitions");

            migrationBuilder.DropIndex(
                name: "IX_Definitions_ContentDefinitionHash",
                table: "Definitions");

            migrationBuilder.DropIndex(
                name: "IX_Definitions_ContentMutationHash",
                table: "Definitions");

            migrationBuilder.DropColumn(
                name: "ContentDefinitionHash",
                table: "Definitions");

            migrationBuilder.RenameColumn(
                name: "ContentMutationHash",
                table: "Definitions",
                newName: "Key");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Key",
                table: "Definitions",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldNullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Key",
                table: "Data",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Definitions",
                table: "Contents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mutations",
                table: "Contents",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Key",
                table: "Data");

            migrationBuilder.DropColumn(
                name: "Definitions",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "Mutations",
                table: "Contents");

            migrationBuilder.RenameColumn(
                name: "Key",
                table: "Definitions",
                newName: "ContentMutationHash");

            migrationBuilder.AlterColumn<byte[]>(
                name: "ContentMutationHash",
                table: "Definitions",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldNullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ContentDefinitionHash",
                table: "Definitions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Definitions_ContentDefinitionHash",
                table: "Definitions",
                column: "ContentDefinitionHash");

            migrationBuilder.CreateIndex(
                name: "IX_Definitions_ContentMutationHash",
                table: "Definitions",
                column: "ContentMutationHash");

            migrationBuilder.AddForeignKey(
                name: "FK_Definitions_Contents_ContentDefinitionHash",
                table: "Definitions",
                column: "ContentDefinitionHash",
                principalTable: "Contents",
                principalColumn: "Hash",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Definitions_Contents_ContentMutationHash",
                table: "Definitions",
                column: "ContentMutationHash",
                principalTable: "Contents",
                principalColumn: "Hash",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

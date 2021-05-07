using Microsoft.EntityFrameworkCore.Migrations;

namespace AirVinyl.API.Migrations
{
    public partial class DynamicProperty3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DynamicProperty_VinylRecords_VinylRecordId",
                table: "DynamicProperty");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DynamicProperty",
                table: "DynamicProperty");

            migrationBuilder.RenameTable(
                name: "DynamicProperty",
                newName: "DynamicVinylRecordProperties");

            migrationBuilder.RenameIndex(
                name: "IX_DynamicProperty_VinylRecordId",
                table: "DynamicVinylRecordProperties",
                newName: "IX_DynamicVinylRecordProperties_VinylRecordId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DynamicVinylRecordProperties",
                table: "DynamicVinylRecordProperties",
                columns: new[] { "Key", "VinylRecordId" });

            migrationBuilder.AddForeignKey(
                name: "FK_DynamicVinylRecordProperties_VinylRecords_VinylRecordId",
                table: "DynamicVinylRecordProperties",
                column: "VinylRecordId",
                principalTable: "VinylRecords",
                principalColumn: "VinylRecordId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DynamicVinylRecordProperties_VinylRecords_VinylRecordId",
                table: "DynamicVinylRecordProperties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DynamicVinylRecordProperties",
                table: "DynamicVinylRecordProperties");

            migrationBuilder.RenameTable(
                name: "DynamicVinylRecordProperties",
                newName: "DynamicProperty");

            migrationBuilder.RenameIndex(
                name: "IX_DynamicVinylRecordProperties_VinylRecordId",
                table: "DynamicProperty",
                newName: "IX_DynamicProperty_VinylRecordId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DynamicProperty",
                table: "DynamicProperty",
                columns: new[] { "Key", "VinylRecordId" });

            migrationBuilder.AddForeignKey(
                name: "FK_DynamicProperty_VinylRecords_VinylRecordId",
                table: "DynamicProperty",
                column: "VinylRecordId",
                principalTable: "VinylRecords",
                principalColumn: "VinylRecordId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

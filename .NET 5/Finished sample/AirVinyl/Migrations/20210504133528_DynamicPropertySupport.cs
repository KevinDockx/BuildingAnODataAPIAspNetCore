using Microsoft.EntityFrameworkCore.Migrations;

namespace AirVinyl.Migrations
{
    public partial class DynamicPropertySupport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DynamicVinylRecordProperties",
                columns: table => new
                {
                    Key = table.Column<string>(nullable: false),
                    VinylRecordId = table.Column<int>(nullable: false),
                    SerializedValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DynamicVinylRecordProperties", x => new { x.Key, x.VinylRecordId });
                    table.ForeignKey(
                        name: "FK_DynamicVinylRecordProperties_VinylRecords_VinylRecordId",
                        column: x => x.VinylRecordId,
                        principalTable: "VinylRecords",
                        principalColumn: "VinylRecordId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "DynamicVinylRecordProperties",
                columns: new[] { "Key", "VinylRecordId", "SerializedValue" },
                values: new object[] { "Publisher", 1, "\"Geffen\"" });

            migrationBuilder.CreateIndex(
                name: "IX_DynamicVinylRecordProperties_VinylRecordId",
                table: "DynamicVinylRecordProperties",
                column: "VinylRecordId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DynamicVinylRecordProperties");
        }
    }
}

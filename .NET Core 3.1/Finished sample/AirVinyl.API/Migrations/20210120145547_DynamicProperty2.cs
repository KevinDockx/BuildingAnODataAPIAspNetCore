using Microsoft.EntityFrameworkCore.Migrations;

namespace AirVinyl.API.Migrations
{
    public partial class DynamicProperty2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DynamicProperty",
                columns: table => new
                {
                    Key = table.Column<string>(nullable: false),
                    VinylRecordId = table.Column<int>(nullable: false),
                    SerializedValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DynamicProperty", x => new { x.Key, x.VinylRecordId });
                    table.ForeignKey(
                        name: "FK_DynamicProperty_VinylRecords_VinylRecordId",
                        column: x => x.VinylRecordId,
                        principalTable: "VinylRecords",
                        principalColumn: "VinylRecordId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "DynamicProperty",
                columns: new[] { "Key", "VinylRecordId", "SerializedValue" },
                values: new object[] { "Publisher", 1, "\"Geffen\"" });

            migrationBuilder.CreateIndex(
                name: "IX_DynamicProperty_VinylRecordId",
                table: "DynamicProperty",
                column: "VinylRecordId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DynamicProperty");
        }
    }
}

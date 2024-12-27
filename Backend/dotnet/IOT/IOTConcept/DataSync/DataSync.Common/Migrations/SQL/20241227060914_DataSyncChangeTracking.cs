using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataSync.Common.Migrations.SQL
{
    /// <inheritdoc />
    public partial class DataSyncChangeTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChangeTrackers",
                columns: table => new
                {
                    TableName = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChangeVersion = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeTrackers", x => x.TableName);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChangeTrackers");
        }
    }
}

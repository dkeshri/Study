using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dkeshri.DataSync.Common.Migrations.SQL
{
    /// <inheritdoc />
    public partial class DataSyncChangeTracker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChangeTrackers",
                columns: table => new
                {
                    TableName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ChangeVersion = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChangeTrackers_TableName",
                table: "ChangeTrackers",
                column: "TableName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChangeTrackers");
        }
    }
}

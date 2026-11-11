using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MininalApi.Migrations
{
    /// <inheritdoc />
    public partial class IntialSQLDB3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Product",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedDate",
                table: "Product",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Product");
        }
    }
}

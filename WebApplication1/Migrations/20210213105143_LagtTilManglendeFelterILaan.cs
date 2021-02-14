using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class LagtTilManglendeFelterILaan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Aar",
                table: "Laan",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ForrigeBetaling",
                table: "Laan",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "Innbetalt",
                table: "Laan",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aar",
                table: "Laan");

            migrationBuilder.DropColumn(
                name: "ForrigeBetaling",
                table: "Laan");

            migrationBuilder.DropColumn(
                name: "Innbetalt",
                table: "Laan");
        }
    }
}

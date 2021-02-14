using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class LagtTilLaan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Laan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LaaneDato = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LaaneSum = table.Column<decimal>(type: "TEXT", nullable: false),
                    KundeId = table.Column<int>(type: "INTEGER", nullable: true),
                    LaaneTypeId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Laan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Laan_Kunder_KundeId",
                        column: x => x.KundeId,
                        principalTable: "Kunder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Laan_LaaneTyper_LaaneTypeId",
                        column: x => x.LaaneTypeId,
                        principalTable: "LaaneTyper",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Laan_KundeId",
                table: "Laan",
                column: "KundeId");

            migrationBuilder.CreateIndex(
                name: "IX_Laan_LaaneTypeId",
                table: "Laan",
                column: "LaaneTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Laan");
        }
    }
}

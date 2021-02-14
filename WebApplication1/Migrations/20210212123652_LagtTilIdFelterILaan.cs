using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class LagtTilIdFelterILaan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Laan_Kunder_KundeId",
                table: "Laan");

            migrationBuilder.DropForeignKey(
                name: "FK_Laan_LaaneTyper_LaaneTypeId",
                table: "Laan");

            migrationBuilder.AlterColumn<int>(
                name: "LaaneTypeId",
                table: "Laan",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "KundeId",
                table: "Laan",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Laan_Kunder_KundeId",
                table: "Laan",
                column: "KundeId",
                principalTable: "Kunder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Laan_LaaneTyper_LaaneTypeId",
                table: "Laan",
                column: "LaaneTypeId",
                principalTable: "LaaneTyper",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Laan_Kunder_KundeId",
                table: "Laan");

            migrationBuilder.DropForeignKey(
                name: "FK_Laan_LaaneTyper_LaaneTypeId",
                table: "Laan");

            migrationBuilder.AlterColumn<int>(
                name: "LaaneTypeId",
                table: "Laan",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "KundeId",
                table: "Laan",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Laan_Kunder_KundeId",
                table: "Laan",
                column: "KundeId",
                principalTable: "Kunder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Laan_LaaneTyper_LaaneTypeId",
                table: "Laan",
                column: "LaaneTypeId",
                principalTable: "LaaneTyper",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

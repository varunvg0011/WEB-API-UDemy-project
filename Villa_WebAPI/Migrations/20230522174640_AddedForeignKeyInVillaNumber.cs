using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Villa_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedForeignKeyInVillaNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VillaID",
                table: "VillaNumbers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2023, 5, 22, 23, 16, 39, 882, DateTimeKind.Local).AddTicks(3212));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2023, 5, 22, 23, 16, 39, 882, DateTimeKind.Local).AddTicks(3222));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2023, 5, 22, 23, 16, 39, 882, DateTimeKind.Local).AddTicks(3223));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2023, 5, 22, 23, 16, 39, 882, DateTimeKind.Local).AddTicks(3227));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2023, 5, 22, 23, 16, 39, 882, DateTimeKind.Local).AddTicks(3228));

            migrationBuilder.CreateIndex(
                name: "IX_VillaNumbers_VillaID",
                table: "VillaNumbers",
                column: "VillaID");

            migrationBuilder.AddForeignKey(
                name: "FK_VillaNumbers_Villas_VillaID",
                table: "VillaNumbers",
                column: "VillaID",
                principalTable: "Villas",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VillaNumbers_Villas_VillaID",
                table: "VillaNumbers");

            migrationBuilder.DropIndex(
                name: "IX_VillaNumbers_VillaID",
                table: "VillaNumbers");

            migrationBuilder.DropColumn(
                name: "VillaID",
                table: "VillaNumbers");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2023, 5, 21, 5, 0, 56, 743, DateTimeKind.Local).AddTicks(617));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2023, 5, 21, 5, 0, 56, 743, DateTimeKind.Local).AddTicks(628));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2023, 5, 21, 5, 0, 56, 743, DateTimeKind.Local).AddTicks(629));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2023, 5, 21, 5, 0, 56, 743, DateTimeKind.Local).AddTicks(630));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2023, 5, 21, 5, 0, 56, 743, DateTimeKind.Local).AddTicks(633));
        }
    }
}

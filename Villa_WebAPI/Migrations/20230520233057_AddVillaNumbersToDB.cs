using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Villa_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddVillaNumbersToDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VillaNumbers",
                columns: table => new
                {
                    VillaNo = table.Column<int>(type: "int", nullable: false),
                    SpecialDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VillaNumbers", x => x.VillaNo);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VillaNumbers");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 28, 3, 42, 36, 764, DateTimeKind.Local).AddTicks(3073));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 28, 3, 42, 36, 764, DateTimeKind.Local).AddTicks(3095));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 28, 3, 42, 36, 764, DateTimeKind.Local).AddTicks(3096));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 28, 3, 42, 36, 764, DateTimeKind.Local).AddTicks(3098));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2023, 4, 28, 3, 42, 36, 764, DateTimeKind.Local).AddTicks(3099));
        }
    }
}

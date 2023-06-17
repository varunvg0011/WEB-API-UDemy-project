using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Villa_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedNamePropertyInAppUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2023, 6, 17, 12, 16, 51, 709, DateTimeKind.Local).AddTicks(893));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2023, 6, 17, 12, 16, 51, 709, DateTimeKind.Local).AddTicks(904));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2023, 6, 17, 12, 16, 51, 709, DateTimeKind.Local).AddTicks(906));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2023, 6, 17, 12, 16, 51, 709, DateTimeKind.Local).AddTicks(907));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2023, 6, 17, 12, 16, 51, 709, DateTimeKind.Local).AddTicks(909));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2023, 6, 16, 14, 2, 41, 380, DateTimeKind.Local).AddTicks(5271));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2023, 6, 16, 14, 2, 41, 380, DateTimeKind.Local).AddTicks(5283));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2023, 6, 16, 14, 2, 41, 380, DateTimeKind.Local).AddTicks(5285));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2023, 6, 16, 14, 2, 41, 380, DateTimeKind.Local).AddTicks(5287));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2023, 6, 16, 14, 2, 41, 380, DateTimeKind.Local).AddTicks(5288));
        }
    }
}

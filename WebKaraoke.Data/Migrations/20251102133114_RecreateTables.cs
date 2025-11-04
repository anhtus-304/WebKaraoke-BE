using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebKaraoke.Data.Migrations
{
    /// <inheritdoc />
    public partial class RecreateTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "KhuyenMai",
                keyColumn: "KM_ID",
                keyValue: 1,
                columns: new[] { "NgayBatDau", "NgayKetThuc" },
                values: new object[] { new DateTime(2025, 10, 23, 20, 31, 13, 262, DateTimeKind.Local).AddTicks(3243), new DateTime(2025, 11, 22, 20, 31, 13, 262, DateTimeKind.Local).AddTicks(3256) });

            migrationBuilder.UpdateData(
                table: "KhuyenMai",
                keyColumn: "KM_ID",
                keyValue: 2,
                columns: new[] { "NgayBatDau", "NgayKetThuc" },
                values: new object[] { new DateTime(2025, 10, 28, 20, 31, 13, 262, DateTimeKind.Local).AddTicks(3258), new DateTime(2025, 12, 2, 20, 31, 13, 262, DateTimeKind.Local).AddTicks(3259) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "KhuyenMai",
                keyColumn: "KM_ID",
                keyValue: 1,
                columns: new[] { "NgayBatDau", "NgayKetThuc" },
                values: new object[] { new DateTime(2025, 10, 18, 16, 6, 20, 391, DateTimeKind.Local).AddTicks(810), new DateTime(2025, 11, 17, 16, 6, 20, 391, DateTimeKind.Local).AddTicks(822) });

            migrationBuilder.UpdateData(
                table: "KhuyenMai",
                keyColumn: "KM_ID",
                keyValue: 2,
                columns: new[] { "NgayBatDau", "NgayKetThuc" },
                values: new object[] { new DateTime(2025, 10, 23, 16, 6, 20, 391, DateTimeKind.Local).AddTicks(825), new DateTime(2025, 11, 27, 16, 6, 20, 391, DateTimeKind.Local).AddTicks(826) });
        }
    }
}

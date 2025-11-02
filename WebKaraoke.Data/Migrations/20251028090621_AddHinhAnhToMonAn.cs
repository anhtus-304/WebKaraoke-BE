using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebKaraoke.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddHinhAnhToMonAn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietHoaDons_HoaDons_HoaDonID",
                table: "ChiTietHoaDons");

            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietHoaDons_MonAnNuocUongs_MonID",
                table: "ChiTietHoaDons");

            migrationBuilder.DropForeignKey(
                name: "FK_DatPhongs_KhachHang_KhachHangID",
                table: "DatPhongs");

            migrationBuilder.DropForeignKey(
                name: "FK_DatPhongs_Phong_PhongID",
                table: "DatPhongs");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDons_DatPhongs_DatPhongID",
                table: "HoaDons");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDons_KhuyenMais_KM_ID",
                table: "HoaDons");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDons_NhanViens_NhanVienID",
                table: "HoaDons");

            migrationBuilder.DropForeignKey(
                name: "FK_LichSuDiems_DiemThanhViens_DiemID",
                table: "LichSuDiems");

            migrationBuilder.DropForeignKey(
                name: "FK_TaiKhoan_NhanViens_NhanVienID",
                table: "TaiKhoan");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NhanViens",
                table: "NhanViens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MonAnNuocUongs",
                table: "MonAnNuocUongs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LichSuDiems",
                table: "LichSuDiems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KhuyenMais",
                table: "KhuyenMais");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HoaDons",
                table: "HoaDons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DatPhongs",
                table: "DatPhongs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChiTietHoaDons",
                table: "ChiTietHoaDons");

            migrationBuilder.RenameTable(
                name: "NhanViens",
                newName: "NhanVien");

            migrationBuilder.RenameTable(
                name: "MonAnNuocUongs",
                newName: "MonAnNuocUong");

            migrationBuilder.RenameTable(
                name: "LichSuDiems",
                newName: "LichSuDiem");

            migrationBuilder.RenameTable(
                name: "KhuyenMais",
                newName: "KhuyenMai");

            migrationBuilder.RenameTable(
                name: "HoaDons",
                newName: "HoaDon");

            migrationBuilder.RenameTable(
                name: "DatPhongs",
                newName: "DatPhong");

            migrationBuilder.RenameTable(
                name: "ChiTietHoaDons",
                newName: "ChiTietHoaDon");

            migrationBuilder.RenameIndex(
                name: "IX_LichSuDiems_DiemID",
                table: "LichSuDiem",
                newName: "IX_LichSuDiem_DiemID");

            migrationBuilder.RenameIndex(
                name: "IX_KhuyenMais_MaKM",
                table: "KhuyenMai",
                newName: "IX_KhuyenMai_MaKM");

            migrationBuilder.RenameIndex(
                name: "IX_HoaDons_NhanVienID",
                table: "HoaDon",
                newName: "IX_HoaDon_NhanVienID");

            migrationBuilder.RenameIndex(
                name: "IX_HoaDons_KM_ID",
                table: "HoaDon",
                newName: "IX_HoaDon_KM_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HoaDons_DatPhongID",
                table: "HoaDon",
                newName: "IX_HoaDon_DatPhongID");

            migrationBuilder.RenameIndex(
                name: "IX_DatPhongs_PhongID",
                table: "DatPhong",
                newName: "IX_DatPhong_PhongID");

            migrationBuilder.RenameIndex(
                name: "IX_DatPhongs_KhachHangID",
                table: "DatPhong",
                newName: "IX_DatPhong_KhachHangID");

            migrationBuilder.RenameIndex(
                name: "IX_ChiTietHoaDons_MonID",
                table: "ChiTietHoaDon",
                newName: "IX_ChiTietHoaDon_MonID");

            migrationBuilder.RenameIndex(
                name: "IX_ChiTietHoaDons_HoaDonID",
                table: "ChiTietHoaDon",
                newName: "IX_ChiTietHoaDon_HoaDonID");

            migrationBuilder.AddColumn<string>(
                name: "HinhAnhUrl",
                table: "MonAnNuocUong",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_NhanVien",
                table: "NhanVien",
                column: "NhanVienID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MonAnNuocUong",
                table: "MonAnNuocUong",
                column: "MonID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LichSuDiem",
                table: "LichSuDiem",
                column: "LichSuID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KhuyenMai",
                table: "KhuyenMai",
                column: "KM_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HoaDon",
                table: "HoaDon",
                column: "HoaDonID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DatPhong",
                table: "DatPhong",
                column: "DatPhongID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChiTietHoaDon",
                table: "ChiTietHoaDon",
                column: "CTHD_ID");

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

            migrationBuilder.UpdateData(
                table: "MonAnNuocUong",
                keyColumn: "MonID",
                keyValue: 1,
                column: "HinhAnhUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "MonAnNuocUong",
                keyColumn: "MonID",
                keyValue: 2,
                column: "HinhAnhUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "MonAnNuocUong",
                keyColumn: "MonID",
                keyValue: 3,
                column: "HinhAnhUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "MonAnNuocUong",
                keyColumn: "MonID",
                keyValue: 4,
                column: "HinhAnhUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "MonAnNuocUong",
                keyColumn: "MonID",
                keyValue: 5,
                column: "HinhAnhUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "MonAnNuocUong",
                keyColumn: "MonID",
                keyValue: 6,
                column: "HinhAnhUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "MonAnNuocUong",
                keyColumn: "MonID",
                keyValue: 7,
                column: "HinhAnhUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "MonAnNuocUong",
                keyColumn: "MonID",
                keyValue: 8,
                column: "HinhAnhUrl",
                value: null);

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietHoaDon_HoaDon_HoaDonID",
                table: "ChiTietHoaDon",
                column: "HoaDonID",
                principalTable: "HoaDon",
                principalColumn: "HoaDonID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietHoaDon_MonAnNuocUong_MonID",
                table: "ChiTietHoaDon",
                column: "MonID",
                principalTable: "MonAnNuocUong",
                principalColumn: "MonID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DatPhong_KhachHang_KhachHangID",
                table: "DatPhong",
                column: "KhachHangID",
                principalTable: "KhachHang",
                principalColumn: "KhachHangID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DatPhong_Phong_PhongID",
                table: "DatPhong",
                column: "PhongID",
                principalTable: "Phong",
                principalColumn: "PhongID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDon_DatPhong_DatPhongID",
                table: "HoaDon",
                column: "DatPhongID",
                principalTable: "DatPhong",
                principalColumn: "DatPhongID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDon_KhuyenMai_KM_ID",
                table: "HoaDon",
                column: "KM_ID",
                principalTable: "KhuyenMai",
                principalColumn: "KM_ID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDon_NhanVien_NhanVienID",
                table: "HoaDon",
                column: "NhanVienID",
                principalTable: "NhanVien",
                principalColumn: "NhanVienID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LichSuDiem_DiemThanhViens_DiemID",
                table: "LichSuDiem",
                column: "DiemID",
                principalTable: "DiemThanhViens",
                principalColumn: "DiemID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaiKhoan_NhanVien_NhanVienID",
                table: "TaiKhoan",
                column: "NhanVienID",
                principalTable: "NhanVien",
                principalColumn: "NhanVienID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietHoaDon_HoaDon_HoaDonID",
                table: "ChiTietHoaDon");

            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietHoaDon_MonAnNuocUong_MonID",
                table: "ChiTietHoaDon");

            migrationBuilder.DropForeignKey(
                name: "FK_DatPhong_KhachHang_KhachHangID",
                table: "DatPhong");

            migrationBuilder.DropForeignKey(
                name: "FK_DatPhong_Phong_PhongID",
                table: "DatPhong");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDon_DatPhong_DatPhongID",
                table: "HoaDon");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDon_KhuyenMai_KM_ID",
                table: "HoaDon");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDon_NhanVien_NhanVienID",
                table: "HoaDon");

            migrationBuilder.DropForeignKey(
                name: "FK_LichSuDiem_DiemThanhViens_DiemID",
                table: "LichSuDiem");

            migrationBuilder.DropForeignKey(
                name: "FK_TaiKhoan_NhanVien_NhanVienID",
                table: "TaiKhoan");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NhanVien",
                table: "NhanVien");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MonAnNuocUong",
                table: "MonAnNuocUong");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LichSuDiem",
                table: "LichSuDiem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KhuyenMai",
                table: "KhuyenMai");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HoaDon",
                table: "HoaDon");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DatPhong",
                table: "DatPhong");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChiTietHoaDon",
                table: "ChiTietHoaDon");

            migrationBuilder.DropColumn(
                name: "HinhAnhUrl",
                table: "MonAnNuocUong");

            migrationBuilder.RenameTable(
                name: "NhanVien",
                newName: "NhanViens");

            migrationBuilder.RenameTable(
                name: "MonAnNuocUong",
                newName: "MonAnNuocUongs");

            migrationBuilder.RenameTable(
                name: "LichSuDiem",
                newName: "LichSuDiems");

            migrationBuilder.RenameTable(
                name: "KhuyenMai",
                newName: "KhuyenMais");

            migrationBuilder.RenameTable(
                name: "HoaDon",
                newName: "HoaDons");

            migrationBuilder.RenameTable(
                name: "DatPhong",
                newName: "DatPhongs");

            migrationBuilder.RenameTable(
                name: "ChiTietHoaDon",
                newName: "ChiTietHoaDons");

            migrationBuilder.RenameIndex(
                name: "IX_LichSuDiem_DiemID",
                table: "LichSuDiems",
                newName: "IX_LichSuDiems_DiemID");

            migrationBuilder.RenameIndex(
                name: "IX_KhuyenMai_MaKM",
                table: "KhuyenMais",
                newName: "IX_KhuyenMais_MaKM");

            migrationBuilder.RenameIndex(
                name: "IX_HoaDon_NhanVienID",
                table: "HoaDons",
                newName: "IX_HoaDons_NhanVienID");

            migrationBuilder.RenameIndex(
                name: "IX_HoaDon_KM_ID",
                table: "HoaDons",
                newName: "IX_HoaDons_KM_ID");

            migrationBuilder.RenameIndex(
                name: "IX_HoaDon_DatPhongID",
                table: "HoaDons",
                newName: "IX_HoaDons_DatPhongID");

            migrationBuilder.RenameIndex(
                name: "IX_DatPhong_PhongID",
                table: "DatPhongs",
                newName: "IX_DatPhongs_PhongID");

            migrationBuilder.RenameIndex(
                name: "IX_DatPhong_KhachHangID",
                table: "DatPhongs",
                newName: "IX_DatPhongs_KhachHangID");

            migrationBuilder.RenameIndex(
                name: "IX_ChiTietHoaDon_MonID",
                table: "ChiTietHoaDons",
                newName: "IX_ChiTietHoaDons_MonID");

            migrationBuilder.RenameIndex(
                name: "IX_ChiTietHoaDon_HoaDonID",
                table: "ChiTietHoaDons",
                newName: "IX_ChiTietHoaDons_HoaDonID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NhanViens",
                table: "NhanViens",
                column: "NhanVienID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MonAnNuocUongs",
                table: "MonAnNuocUongs",
                column: "MonID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LichSuDiems",
                table: "LichSuDiems",
                column: "LichSuID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KhuyenMais",
                table: "KhuyenMais",
                column: "KM_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HoaDons",
                table: "HoaDons",
                column: "HoaDonID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DatPhongs",
                table: "DatPhongs",
                column: "DatPhongID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChiTietHoaDons",
                table: "ChiTietHoaDons",
                column: "CTHD_ID");

            migrationBuilder.UpdateData(
                table: "KhuyenMais",
                keyColumn: "KM_ID",
                keyValue: 1,
                columns: new[] { "NgayBatDau", "NgayKetThuc" },
                values: new object[] { new DateTime(2025, 10, 18, 15, 19, 49, 521, DateTimeKind.Local).AddTicks(7050), new DateTime(2025, 11, 17, 15, 19, 49, 521, DateTimeKind.Local).AddTicks(7065) });

            migrationBuilder.UpdateData(
                table: "KhuyenMais",
                keyColumn: "KM_ID",
                keyValue: 2,
                columns: new[] { "NgayBatDau", "NgayKetThuc" },
                values: new object[] { new DateTime(2025, 10, 23, 15, 19, 49, 521, DateTimeKind.Local).AddTicks(7067), new DateTime(2025, 11, 27, 15, 19, 49, 521, DateTimeKind.Local).AddTicks(7068) });

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietHoaDons_HoaDons_HoaDonID",
                table: "ChiTietHoaDons",
                column: "HoaDonID",
                principalTable: "HoaDons",
                principalColumn: "HoaDonID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietHoaDons_MonAnNuocUongs_MonID",
                table: "ChiTietHoaDons",
                column: "MonID",
                principalTable: "MonAnNuocUongs",
                principalColumn: "MonID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DatPhongs_KhachHang_KhachHangID",
                table: "DatPhongs",
                column: "KhachHangID",
                principalTable: "KhachHang",
                principalColumn: "KhachHangID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DatPhongs_Phong_PhongID",
                table: "DatPhongs",
                column: "PhongID",
                principalTable: "Phong",
                principalColumn: "PhongID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDons_DatPhongs_DatPhongID",
                table: "HoaDons",
                column: "DatPhongID",
                principalTable: "DatPhongs",
                principalColumn: "DatPhongID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDons_KhuyenMais_KM_ID",
                table: "HoaDons",
                column: "KM_ID",
                principalTable: "KhuyenMais",
                principalColumn: "KM_ID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDons_NhanViens_NhanVienID",
                table: "HoaDons",
                column: "NhanVienID",
                principalTable: "NhanViens",
                principalColumn: "NhanVienID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LichSuDiems_DiemThanhViens_DiemID",
                table: "LichSuDiems",
                column: "DiemID",
                principalTable: "DiemThanhViens",
                principalColumn: "DiemID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaiKhoan_NhanViens_NhanVienID",
                table: "TaiKhoan",
                column: "NhanVienID",
                principalTable: "NhanViens",
                principalColumn: "NhanVienID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

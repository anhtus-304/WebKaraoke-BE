using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebKaraoke.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDiemThanhVien : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KhachHang",
                columns: table => new
                {
                    KhachHangID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    MatKhau = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhachHang", x => x.KhachHangID);
                });

            migrationBuilder.CreateTable(
                name: "KhuyenMais",
                columns: table => new
                {
                    KM_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaKM = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TyLeGiam = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhuyenMais", x => x.KM_ID);
                });

            migrationBuilder.CreateTable(
                name: "MonAnNuocUongs",
                columns: table => new
                {
                    MonID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenMon = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    DanhMuc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    HinhAnh = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DangKinhDoanh = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonAnNuocUongs", x => x.MonID);
                });

            migrationBuilder.CreateTable(
                name: "NhanViens",
                columns: table => new
                {
                    NhanVienID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ChucVu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhanViens", x => x.NhanVienID);
                });

            migrationBuilder.CreateTable(
                name: "Phong",
                columns: table => new
                {
                    PhongID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenPhong = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LoaiPhong = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GiaGio = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phong", x => x.PhongID);
                });

            migrationBuilder.CreateTable(
                name: "DiemThanhViens",
                columns: table => new
                {
                    DiemID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KhachHangID = table.Column<int>(type: "int", nullable: false),
                    TongDiem = table.Column<int>(type: "int", nullable: false),
                    DiemDaSuDung = table.Column<int>(type: "int", nullable: false),
                    NgayCapNhat = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiemThanhViens", x => x.DiemID);
                    table.ForeignKey(
                        name: "FK_DiemThanhViens_KhachHang_KhachHangID",
                        column: x => x.KhachHangID,
                        principalTable: "KhachHang",
                        principalColumn: "KhachHangID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaiKhoan",
                columns: table => new
                {
                    TaiKhoanID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MatKhauHash = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    KhachHangID = table.Column<int>(type: "int", nullable: true),
                    NhanVienID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiKhoan", x => x.TaiKhoanID);
                    table.ForeignKey(
                        name: "FK_TaiKhoan_KhachHang_KhachHangID",
                        column: x => x.KhachHangID,
                        principalTable: "KhachHang",
                        principalColumn: "KhachHangID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaiKhoan_NhanViens_NhanVienID",
                        column: x => x.NhanVienID,
                        principalTable: "NhanViens",
                        principalColumn: "NhanVienID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DatPhongs",
                columns: table => new
                {
                    DatPhongID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KhachHangID = table.Column<int>(type: "int", nullable: false),
                    PhongID = table.Column<int>(type: "int", nullable: false),
                    ThoiGianDat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GioBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GioKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoLuongNguoi = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatPhongs", x => x.DatPhongID);
                    table.ForeignKey(
                        name: "FK_DatPhongs_KhachHang_KhachHangID",
                        column: x => x.KhachHangID,
                        principalTable: "KhachHang",
                        principalColumn: "KhachHangID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DatPhongs_Phong_PhongID",
                        column: x => x.PhongID,
                        principalTable: "Phong",
                        principalColumn: "PhongID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LichSuDiems",
                columns: table => new
                {
                    LichSuID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiemID = table.Column<int>(type: "int", nullable: false),
                    SoDiemThayDoi = table.Column<int>(type: "int", nullable: false),
                    LoaiGiaoDich = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ThoiGian = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichSuDiems", x => x.LichSuID);
                    table.ForeignKey(
                        name: "FK_LichSuDiems_DiemThanhViens_DiemID",
                        column: x => x.DiemID,
                        principalTable: "DiemThanhViens",
                        principalColumn: "DiemID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HoaDons",
                columns: table => new
                {
                    HoaDonID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatPhongID = table.Column<int>(type: "int", nullable: false),
                    NhanVienID = table.Column<int>(type: "int", nullable: false),
                    KM_ID = table.Column<int>(type: "int", nullable: true),
                    NgayLap = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDons", x => x.HoaDonID);
                    table.ForeignKey(
                        name: "FK_HoaDons_DatPhongs_DatPhongID",
                        column: x => x.DatPhongID,
                        principalTable: "DatPhongs",
                        principalColumn: "DatPhongID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HoaDons_KhuyenMais_KM_ID",
                        column: x => x.KM_ID,
                        principalTable: "KhuyenMais",
                        principalColumn: "KM_ID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_HoaDons_NhanViens_NhanVienID",
                        column: x => x.NhanVienID,
                        principalTable: "NhanViens",
                        principalColumn: "NhanVienID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietHoaDons",
                columns: table => new
                {
                    CTHD_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoaDonID = table.Column<int>(type: "int", nullable: false),
                    MonID = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietHoaDons", x => x.CTHD_ID);
                    table.ForeignKey(
                        name: "FK_ChiTietHoaDons_HoaDons_HoaDonID",
                        column: x => x.HoaDonID,
                        principalTable: "HoaDons",
                        principalColumn: "HoaDonID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietHoaDons_MonAnNuocUongs_MonID",
                        column: x => x.MonID,
                        principalTable: "MonAnNuocUongs",
                        principalColumn: "MonID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "KhuyenMais",
                columns: new[] { "KM_ID", "MaKM", "MoTa", "NgayBatDau", "NgayKetThuc", "TyLeGiam" },
                values: new object[,]
                {
                    { 1, "KHAI_TRUONG", "Khuyến mãi khai trương", new DateTime(2025, 10, 18, 15, 19, 49, 521, DateTimeKind.Local).AddTicks(7050), new DateTime(2025, 11, 17, 15, 19, 49, 521, DateTimeKind.Local).AddTicks(7065), 20.00m },
                    { 2, "CUOI_TUAN", "Khuyến mãi cuối tuần", new DateTime(2025, 10, 23, 15, 19, 49, 521, DateTimeKind.Local).AddTicks(7067), new DateTime(2025, 11, 27, 15, 19, 49, 521, DateTimeKind.Local).AddTicks(7068), 10.00m }
                });

            migrationBuilder.InsertData(
                table: "MonAnNuocUongs",
                columns: new[] { "MonID", "DangKinhDoanh", "DanhMuc", "DonGia", "HinhAnh", "MoTa", "TenMon" },
                values: new object[,]
                {
                    { 1, true, "Đồ ăn", 50000m, null, null, "Bò khô" },
                    { 2, true, "Đồ ăn", 40000m, null, null, "Hạt điều" },
                    { 3, true, "Đồ ăn", 35000m, null, null, "Khoai tây chiên" },
                    { 4, true, "Nước uống", 25000m, null, null, "Coca Cola" },
                    { 5, true, "Nước uống", 25000m, null, null, "Pepsi" },
                    { 6, true, "Nước uống", 15000m, null, null, "Nước suối" },
                    { 7, true, "Nước uống", 35000m, null, null, "Trà đào" },
                    { 8, true, "Nước uống", 30000m, null, null, "Cà phê" }
                });

            migrationBuilder.InsertData(
                table: "NhanViens",
                columns: new[] { "NhanVienID", "ChucVu", "HoTen", "SoDienThoai" },
                values: new object[,]
                {
                    { 1, "Quản lý", "Nguyễn Văn A", "0901234567" },
                    { 2, "Nhân viên", "Trần Thị B", "0901234568" }
                });

            migrationBuilder.InsertData(
                table: "Phong",
                columns: new[] { "PhongID", "GiaGio", "LoaiPhong", "TenPhong", "TrangThai" },
                values: new object[,]
                {
                    { 1, 150000m, "VIP", "P001", "Trong" },
                    { 2, 150000m, "VIP", "P002", "Trong" },
                    { 3, 100000m, "Thuong", "P003", "Trong" },
                    { 4, 100000m, "Thuong", "P004", "Trong" }
                });

            migrationBuilder.InsertData(
                table: "TaiKhoan",
                columns: new[] { "TaiKhoanID", "KhachHangID", "MatKhauHash", "NhanVienID", "Role", "Username" },
                values: new object[] { 1, null, "jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=", null, "Admin", "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDons_HoaDonID",
                table: "ChiTietHoaDons",
                column: "HoaDonID");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDons_MonID",
                table: "ChiTietHoaDons",
                column: "MonID");

            migrationBuilder.CreateIndex(
                name: "IX_DatPhongs_KhachHangID",
                table: "DatPhongs",
                column: "KhachHangID");

            migrationBuilder.CreateIndex(
                name: "IX_DatPhongs_PhongID",
                table: "DatPhongs",
                column: "PhongID");

            migrationBuilder.CreateIndex(
                name: "IX_DiemThanhViens_KhachHangID",
                table: "DiemThanhViens",
                column: "KhachHangID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HoaDons_DatPhongID",
                table: "HoaDons",
                column: "DatPhongID");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDons_KM_ID",
                table: "HoaDons",
                column: "KM_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDons_NhanVienID",
                table: "HoaDons",
                column: "NhanVienID");

            migrationBuilder.CreateIndex(
                name: "IX_KhachHang_Email",
                table: "KhachHang",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_KhachHang_SoDienThoai",
                table: "KhachHang",
                column: "SoDienThoai",
                unique: true,
                filter: "[SoDienThoai] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_KhuyenMais_MaKM",
                table: "KhuyenMais",
                column: "MaKM",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LichSuDiems_DiemID",
                table: "LichSuDiems",
                column: "DiemID");

            migrationBuilder.CreateIndex(
                name: "IX_Phong_TenPhong",
                table: "Phong",
                column: "TenPhong",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoan_KhachHangID",
                table: "TaiKhoan",
                column: "KhachHangID",
                unique: true,
                filter: "[KhachHangID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoan_NhanVienID",
                table: "TaiKhoan",
                column: "NhanVienID",
                unique: true,
                filter: "[NhanVienID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoan_Username",
                table: "TaiKhoan",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietHoaDons");

            migrationBuilder.DropTable(
                name: "LichSuDiems");

            migrationBuilder.DropTable(
                name: "TaiKhoan");

            migrationBuilder.DropTable(
                name: "HoaDons");

            migrationBuilder.DropTable(
                name: "MonAnNuocUongs");

            migrationBuilder.DropTable(
                name: "DiemThanhViens");

            migrationBuilder.DropTable(
                name: "DatPhongs");

            migrationBuilder.DropTable(
                name: "KhuyenMais");

            migrationBuilder.DropTable(
                name: "NhanViens");

            migrationBuilder.DropTable(
                name: "KhachHang");

            migrationBuilder.DropTable(
                name: "Phong");
        }
    }
}

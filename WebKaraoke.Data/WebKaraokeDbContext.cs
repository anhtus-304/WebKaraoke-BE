using Microsoft.EntityFrameworkCore;
using WebKaraoke.Data.Entities;

namespace WebKaraoke.Data
{
    public class WebKaraokeDbContext : DbContext
    {
        public WebKaraokeDbContext(DbContextOptions<WebKaraokeDbContext> options) : base(options) { }

        // DbSets - THÊM DÒNG NÀY
        public DbSet<LoaiPhong> LoaiPhongs => Set<LoaiPhong>();

        // Các DbSets hiện có
        public DbSet<KhachHang> KhachHangs => Set<KhachHang>();
        public DbSet<Phong> Phongs => Set<Phong>();
        public DbSet<DatPhong> DatPhongs => Set<DatPhong>();
        public DbSet<NhanVien> NhanViens => Set<NhanVien>();
        public DbSet<KhuyenMai> KhuyenMais => Set<KhuyenMai>();
        public DbSet<HoaDon> HoaDons => Set<HoaDon>();
        public DbSet<MonAnNuocUong> MonAnNuocUongs => Set<MonAnNuocUong>();
        public DbSet<ChiTietHoaDon> ChiTietHoaDons => Set<ChiTietHoaDon>();
        public DbSet<TaiKhoan> TaiKhoans => Set<TaiKhoan>();
        public DbSet<DiemThanhVien> DiemThanhViens => Set<DiemThanhVien>();
        public DbSet<LichSuDiem> LichSuDiems => Set<LichSuDiem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<KhachHang>()
                .ToTable(tb => tb.UseSqlOutputClause(false));

            // THÊM CONFIGURATION CHO LoaiPhong
            modelBuilder.Entity<LoaiPhong>(entity =>
            {
                entity.HasKey(e => e.LoaiPhongID);
                entity.Property(e => e.TenLoai).HasMaxLength(100).IsRequired();
                entity.Property(e => e.GiaPhong).HasColumnType("decimal(18,2)");
                entity.Property(e => e.MoTa).HasMaxLength(500);
                
                entity.HasIndex(e => e.TenLoai).IsUnique();
            });

            // SỬA LẠI Configure Phong - THÊM QUAN HỆ VỚI LoaiPhong
            modelBuilder.Entity<Phong>(entity =>
            {
                entity.HasKey(e => e.PhongID);
                entity.Property(e => e.TenPhong).HasMaxLength(100).IsRequired();
                entity.Property(e => e.TrangThai).HasMaxLength(20).IsRequired();
                
                // Quan hệ với LoaiPhong
                entity.HasOne(e => e.LoaiPhong)
                    .WithMany(l => l.Phongs)
                    .HasForeignKey(e => e.LoaiPhongID)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasIndex(e => e.TenPhong).IsUnique();
            });

            // Configure KhachHang
            modelBuilder.Entity<KhachHang>(entity =>
            {
                entity.HasKey(e => e.KhachHangID);
                entity.Property(e => e.HoTen).HasMaxLength(100).IsRequired();
                entity.Property(e => e.SoDienThoai).HasMaxLength(20);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.DiaChi).HasMaxLength(250);
                
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.SoDienThoai).IsUnique();
            });

            // Configure DatPhong
            modelBuilder.Entity<DatPhong>(entity =>
            {
                entity.HasKey(e => e.DatPhongID);
                entity.Property(e => e.TrangThai).HasMaxLength(20).IsRequired();
                
                // Relationships
                entity.HasOne(e => e.KhachHang)
                    .WithMany(k => k.DatPhongs)
                    .HasForeignKey(e => e.KhachHangID)
                    .OnDelete(DeleteBehavior.Restrict);
                    
                entity.HasOne(e => e.Phong)
                    .WithMany(p => p.DatPhongs)
                    .HasForeignKey(e => e.PhongID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure NhanVien
            modelBuilder.Entity<NhanVien>(entity =>
            {
                entity.HasKey(e => e.NhanVienID);
                entity.Property(e => e.HoTen).HasMaxLength(100).IsRequired();
                entity.Property(e => e.SoDienThoai).HasMaxLength(20);
                entity.Property(e => e.ChucVu).HasMaxLength(50);
            });

            // Configure KhuyenMai
            modelBuilder.Entity<KhuyenMai>(entity =>
            {
                entity.HasKey(e => e.KM_ID);
                entity.Property(e => e.MaKM).HasMaxLength(50).IsRequired();
                entity.Property(e => e.TyLeGiam).HasColumnType("decimal(5,2)");
                
                entity.HasIndex(e => e.MaKM).IsUnique();
            });

            // Configure HoaDon
            modelBuilder.Entity<HoaDon>(entity =>
            {
                entity.HasKey(e => e.HoaDonID);
                entity.Property(e => e.TongTien).HasColumnType("decimal(12,2)");
                entity.Property(e => e.TrangThai).HasMaxLength(20).IsRequired();
                
                // Relationships
                entity.HasOne(e => e.DatPhong)
                    .WithMany(d => d.HoaDons)
                    .HasForeignKey(e => e.DatPhongID)
                    .OnDelete(DeleteBehavior.Restrict);
                    
                entity.HasOne(e => e.NhanVien)
                    .WithMany(n => n.HoaDons)
                    .HasForeignKey(e => e.NhanVienID)
                    .OnDelete(DeleteBehavior.Restrict);
                    
                entity.HasOne(e => e.KhuyenMai)
                    .WithMany(k => k.HoaDons)
                    .HasForeignKey(e => e.KM_ID)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure MonAnNuocUong
            modelBuilder.Entity<MonAnNuocUong>(entity =>
            {
                entity.HasKey(e => e.MonID);
                entity.Property(e => e.TenMon).HasMaxLength(150).IsRequired();
                entity.Property(e => e.DonGia).HasColumnType("decimal(10,2)");
                entity.Property(e => e.DanhMuc).HasMaxLength(50).IsRequired();
                entity.Property(e => e.MoTa).HasMaxLength(500);
                entity.Property(e => e.HinhAnh).HasMaxLength(500);
            });

            // Configure ChiTietHoaDon
            modelBuilder.Entity<ChiTietHoaDon>(entity =>
            {
                entity.HasKey(e => e.CTHD_ID);
                entity.Property(e => e.DonGia).HasColumnType("decimal(10,2)");
                
                // Relationships
                entity.HasOne(e => e.HoaDon)
                    .WithMany(h => h.ChiTietHoaDons)
                    .HasForeignKey(e => e.HoaDonID)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.MonAnNuocUong)
                    .WithMany(m => m.ChiTietHoaDons)
                    .HasForeignKey(e => e.MonID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure TaiKhoan
            modelBuilder.Entity<TaiKhoan>(entity =>
            {
                entity.HasKey(e => e.TaiKhoanID);
                entity.Property(e => e.Username).HasMaxLength(100).IsRequired();
                entity.Property(e => e.MatKhauHash).HasMaxLength(200).IsRequired();
                entity.Property(e => e.Role).HasMaxLength(20).IsRequired();
                
                entity.HasIndex(e => e.Username).IsUnique();
                
                // Relationships
                entity.HasOne(e => e.KhachHang)
                    .WithOne(k => k.TaiKhoan)
                    .HasForeignKey<TaiKhoan>(e => e.KhachHangID)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.NhanVien)
                    .WithOne(n => n.TaiKhoan)
                    .HasForeignKey<TaiKhoan>(e => e.NhanVienID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure DiemThanhVien
            modelBuilder.Entity<DiemThanhVien>(entity =>
            {
                entity.HasKey(e => e.DiemID);
                
                // Relationships
                entity.HasOne(e => e.KhachHang)
                    .WithOne(k => k.DiemThanhVien)
                    .HasForeignKey<DiemThanhVien>(e => e.KhachHangID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure LichSuDiem
            modelBuilder.Entity<LichSuDiem>(entity =>
            {
                entity.HasKey(e => e.LichSuID);
                entity.Property(e => e.LoaiGiaoDich).HasMaxLength(50).IsRequired();
                entity.Property(e => e.MoTa).HasMaxLength(250);
                
                // Relationships
                entity.HasOne(e => e.DiemThanhVien)
                    .WithMany(d => d.LichSuDiems)
                    .HasForeignKey(e => e.DiemID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // THÊM SEED DATA CHO LoaiPhong TRƯỚC
            modelBuilder.Entity<LoaiPhong>().HasData(
                new LoaiPhong 
                { 
                    LoaiPhongID = 1, 
                    TenLoai = "VIP", 
                    GiaPhong = 150000, 
                    SucChua = 10, 
                    MoTa = "Phòng VIP cao cấp" 
                },
                new LoaiPhong 
                { 
                    LoaiPhongID = 2, 
                    TenLoai = "Thuong", 
                    GiaPhong = 100000, 
                    SucChua = 6, 
                    MoTa = "Phòng thường" 
                }
            );

            // SỬA LẠI Seed Phongs - SỬ DỤNG LoaiPhongID THAY VÌ LoaiPhong string
            modelBuilder.Entity<Phong>().HasData(
                new Phong { PhongID = 1, TenPhong = "P001", LoaiPhongID = 1, TrangThai = "Trong" },
                new Phong { PhongID = 2, TenPhong = "P002", LoaiPhongID = 1, TrangThai = "Trong" },
                new Phong { PhongID = 3, TenPhong = "P003", LoaiPhongID = 2, TrangThai = "Trong" },
                new Phong { PhongID = 4, TenPhong = "P004", LoaiPhongID = 2, TrangThai = "Trong" }
            );

            // Seed NhanVien
            modelBuilder.Entity<NhanVien>().HasData(
                new NhanVien { NhanVienID = 1, HoTen = "Nguyễn Văn A", SoDienThoai = "0901234567", ChucVu = "Quản lý" },
                new NhanVien { NhanVienID = 2, HoTen = "Trần Thị B", SoDienThoai = "0901234568", ChucVu = "Nhân viên" }
            );

            // Seed Admin account
            modelBuilder.Entity<TaiKhoan>().HasData(
                new TaiKhoan 
                { 
                    TaiKhoanID = 1, 
                    Username = "admin", 
                    MatKhauHash = "jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=", // "admin123"
                    Role = "Admin" 
                }
            );

            // Seed MonAnNuocUong
            modelBuilder.Entity<MonAnNuocUong>().HasData(
                // Đồ ăn
                new MonAnNuocUong { MonID = 1, TenMon = "Bò khô", DonGia = 50000, DanhMuc = "Đồ ăn" },
                new MonAnNuocUong { MonID = 2, TenMon = "Hạt điều", DonGia = 40000, DanhMuc = "Đồ ăn" },
                new MonAnNuocUong { MonID = 3, TenMon = "Khoai tây chiên", DonGia = 35000, DanhMuc = "Đồ ăn" },
                
                // Nước uống
                new MonAnNuocUong { MonID = 4, TenMon = "Coca Cola", DonGia = 25000, DanhMuc = "Nước uống" },
                new MonAnNuocUong { MonID = 5, TenMon = "Pepsi", DonGia = 25000, DanhMuc = "Nước uống" },
                new MonAnNuocUong { MonID = 6, TenMon = "Nước suối", DonGia = 15000, DanhMuc = "Nước uống" },
                new MonAnNuocUong { MonID = 7, TenMon = "Trà đào", DonGia = 35000, DanhMuc = "Nước uống" },
                new MonAnNuocUong { MonID = 8, TenMon = "Cà phê", DonGia = 30000, DanhMuc = "Nước uống" }
            );

            // Seed KhuyenMai
            modelBuilder.Entity<KhuyenMai>().HasData(
                new KhuyenMai 
                { 
                    KM_ID = 1, 
                    MaKM = "KHAI_TRUONG", 
                    TyLeGiam = 20.00m, 
                    NgayBatDau = DateTime.Now.AddDays(-10), 
                    NgayKetThuc = DateTime.Now.AddDays(20),
                    MoTa = "Khuyến mãi khai trương"
                },
                new KhuyenMai 
                { 
                    KM_ID = 2, 
                    MaKM = "CUOI_TUAN", 
                    TyLeGiam = 10.00m, 
                    NgayBatDau = DateTime.Now.AddDays(-5), 
                    NgayKetThuc = DateTime.Now.AddDays(30),
                    MoTa = "Khuyến mãi cuối tuần"
                }
            );
        }
    }
}
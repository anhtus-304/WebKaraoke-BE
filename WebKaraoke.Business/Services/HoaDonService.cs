using AutoMapper;
using WebKaraoke.Data;
using WebKaraoke.Data.Entities;
using WebKaraoke.Business.Interfaces;
using WebKaraoke.DTO;
using Microsoft.Extensions.Logging;
namespace WebKaraoke.Business.Services
{
    public class HoaDonService : IHoaDonService
    {
        private readonly IRepository<HoaDon> _hoaDonRepository;
        private readonly IRepository<ChiTietHoaDon> _chiTietHoaDonRepository;
        private readonly IRepository<DatPhong> _datPhongRepository;
        private readonly IRepository<MonAnNuocUong> _monAnRepository;
        private readonly IRepository<KhuyenMai> _khuyenMaiRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<HoaDonService> _logger;

        public HoaDonService(
            IRepository<HoaDon> hoaDonRepository,
            IRepository<ChiTietHoaDon> chiTietHoaDonRepository,
            IRepository<DatPhong> datPhongRepository,
            IRepository<MonAnNuocUong> monAnRepository,
            IRepository<KhuyenMai> khuyenMaiRepository,
            IMapper mapper,
            ILogger<HoaDonService> logger)
        {
            _hoaDonRepository = hoaDonRepository;
            _chiTietHoaDonRepository = chiTietHoaDonRepository;
            _datPhongRepository = datPhongRepository;
            _monAnRepository = monAnRepository;
            _khuyenMaiRepository = khuyenMaiRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<HoaDonDTO?> GetHoaDonByDatPhongAsync(int datPhongId)
        {
            try
            {
                var hoaDon = (await _hoaDonRepository.GetAllAsync())
                    .FirstOrDefault(hd => hd.DatPhongID == datPhongId);
                
                return _mapper.Map<HoaDonDTO?>(hoaDon);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting invoice for booking: {BookingId}", datPhongId);
                throw;
            }
        }

        public async Task<bool> TaoHoaDonAsync(int datPhongId, int nhanVienId)
        {
            try
            {
                // Check if invoice already exists
                var existingHoaDon = (await _hoaDonRepository.GetAllAsync())
                    .FirstOrDefault(hd => hd.DatPhongID == datPhongId);
                
                if (existingHoaDon != null)
                {
                    _logger.LogWarning("Invoice already exists for booking: {BookingId}", datPhongId);
                    return false;
                }

                var datPhong = await _datPhongRepository.GetByIdAsync(datPhongId);
                if (datPhong == null)
                {
                    _logger.LogWarning("Booking {BookingId} not found", datPhongId);
                    return false;
                }

                // Calculate room cost
                var thoiGianSuDung = datPhong.GioKetThuc - datPhong.GioBatDau;
                var tongGio = (decimal)thoiGianSuDung.TotalHours;
                var tienPhong = tongGio * 150000; // Assuming room price is 150,000 per hour

                var hoaDon = new HoaDon
                {
                    DatPhongID = datPhongId,
                    NhanVienID = nhanVienId,
                    NgayLap = DateTime.UtcNow,
                    TongTien = tienPhong,
                    TrangThai = "ChuaThanhToan"
                };

                await _hoaDonRepository.AddAsync(hoaDon);
                await _hoaDonRepository.SaveAsync();

                _logger.LogInformation("Invoice created successfully for booking: {BookingId}", datPhongId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating invoice for booking: {BookingId}", datPhongId);
                throw;
            }
        }

        public async Task<bool> ThemMonVaoHoaDonAsync(int hoaDonId, int monId, int soLuong)
        {
            try
            {
                var hoaDon = await _hoaDonRepository.GetByIdAsync(hoaDonId);
                var monAn = await _monAnRepository.GetByIdAsync(monId);

                if (hoaDon == null || monAn == null)
                {
                    _logger.LogWarning("Invoice {InvoiceId} or menu item {ItemId} not found", hoaDonId, monId);
                    return false;
                }

                var chiTietHoaDon = new ChiTietHoaDon
                {
                    HoaDonID = hoaDonId,
                    MonID = monId,
                    SoLuong = soLuong,
                    DonGia = monAn.DonGia
                };

                await _chiTietHoaDonRepository.AddAsync(chiTietHoaDon);

                // Update total amount
                hoaDon.TongTien += soLuong * monAn.DonGia;
                _hoaDonRepository.Update(hoaDon);

                await _chiTietHoaDonRepository.SaveAsync();

                _logger.LogInformation("Item {ItemId} added to invoice {InvoiceId} successfully", monId, hoaDonId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding item {ItemId} to invoice {InvoiceId}", monId, hoaDonId);
                throw;
            }
        }

        public async Task<bool> ApDungKhuyenMaiAsync(int hoaDonId, string maKhuyenMai)
        {
            try
            {
                var hoaDon = await _hoaDonRepository.GetByIdAsync(hoaDonId);
                var khuyenMai = (await _khuyenMaiRepository.GetAllAsync())
                    .FirstOrDefault(km => km.MaKM == maKhuyenMai && 
                                         km.NgayBatDau <= DateTime.Now && 
                                         km.NgayKetThuc >= DateTime.Now);

                if (hoaDon == null || khuyenMai == null)
                {
                    _logger.LogWarning("Invoice {InvoiceId} or promotion {PromotionCode} not found or invalid", hoaDonId, maKhuyenMai);
                    return false;
                }

                hoaDon.KM_ID = khuyenMai.KM_ID;
                _hoaDonRepository.Update(hoaDon);
                await _hoaDonRepository.SaveAsync();

                _logger.LogInformation("Promotion {PromotionCode} applied to invoice {InvoiceId} successfully", maKhuyenMai, hoaDonId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error applying promotion {PromotionCode} to invoice {InvoiceId}", maKhuyenMai, hoaDonId);
                throw;
            }
        }

        public async Task<decimal> TinhTongTienHoaDonAsync(int hoaDonId)
        {
            try
            {
                var hoaDon = await _hoaDonRepository.GetByIdAsync(hoaDonId);
                if (hoaDon == null)
                {
                    _logger.LogWarning("Invoice {InvoiceId} not found", hoaDonId);
                    return 0;
                }

                var chiTietHoaDons = (await _chiTietHoaDonRepository.GetAllAsync())
                    .Where(ct => ct.HoaDonID == hoaDonId);

                var tongTienMonAn = chiTietHoaDons.Sum(ct => ct.SoLuong * ct.DonGia);
                var tongTien = hoaDon.TongTien + tongTienMonAn;

                // Apply discount if any
                if (hoaDon.KhuyenMai != null)
                {
                    tongTien -= tongTien * (hoaDon.KhuyenMai.TyLeGiam / 100);
                }

                return tongTien;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating total for invoice: {InvoiceId}", hoaDonId);
                throw;
            }
        }
    }
}
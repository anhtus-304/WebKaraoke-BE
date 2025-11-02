using AutoMapper;
using WebKaraoke.Data.Entities;
using WebKaraoke.DTO;
using WebKaraoke.Business.Mapping;

namespace WebKaraoke.Business.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Phong mappings
            CreateMap<Phong, PhongDTO>();
            CreateMap<PhongCreateDTO, Phong>();
            CreateMap<PhongUpdateDTO, Phong>();

            // KhachHang mappings
            CreateMap<KhachHang, KhachHangDTO>();
            CreateMap<KhachHangCreateDTO, KhachHang>();
            CreateMap<KhachHangUpdateDTO, KhachHang>();

            // DatPhong mappings
            CreateMap<DatPhong, DatPhongDTO>()
                .ForMember(dest => dest.TenPhong, opt => opt.MapFrom(src => src.Phong.TenPhong))
                .ForMember(dest => dest.TenKhachHang, opt => opt.MapFrom(src => src.KhachHang.HoTen))
                .ForMember(dest => dest.LoaiPhong, opt => opt.MapFrom(src => src.Phong.LoaiPhong))
                .ForMember(dest => dest.GiaGio, opt => opt.MapFrom(src => src.Phong.GiaGio));

            CreateMap<DatPhongRequest, DatPhong>();
            CreateMap<DatPhongUpdateDTO, DatPhong>();

            // MonAnNuocUong mappings
            CreateMap<MonAnNuocUong, MonAnNuocUongDTO>();
            CreateMap<MonAnNuocUongCreateDTO, MonAnNuocUong>();
            CreateMap<MonAnNuocUongUpdateDTO, MonAnNuocUong>();

            // HoaDon mappings
            CreateMap<HoaDon, HoaDonDTO>()
                .ForMember(dest => dest.TenNhanVien, opt => opt.MapFrom(src => src.NhanVien.HoTen))
                .ForMember(dest => dest.MaKhuyenMai, opt => opt.MapFrom(src => src.KhuyenMai != null ? src.KhuyenMai.MaKM : ""))
                .ForMember(dest => dest.TyLeGiam, opt => opt.MapFrom(src => src.KhuyenMai != null ? src.KhuyenMai.TyLeGiam : 0))
                .ForMember(dest => dest.TienGiam, opt => opt.MapFrom(src => src.TongTien * (src.KhuyenMai != null ? src.KhuyenMai.TyLeGiam / 100 : 0)))
                .ForMember(dest => dest.ThanhTien, opt => opt.MapFrom(src => src.TongTien - (src.TongTien * (src.KhuyenMai != null ? src.KhuyenMai.TyLeGiam / 100 : 0))));

            CreateMap<HoaDonCreateDTO, HoaDon>();

            // ChiTietHoaDon mappings
            CreateMap<ChiTietHoaDon, ChiTietHoaDonDTO>()
                .ForMember(dest => dest.TenMon, opt => opt.MapFrom(src => src.MonAnNuocUong.TenMon))
                .ForMember(dest => dest.DanhMuc, opt => opt.MapFrom(src => src.MonAnNuocUong.DanhMuc));

            // NhanVien mappings
            CreateMap<NhanVien, NhanVienDTO>();
            CreateMap<NhanVienCreateDTO, NhanVien>();
            CreateMap<NhanVienUpdateDTO, NhanVien>();

            // KhuyenMai mappings
            CreateMap<KhuyenMai, KhuyenMaiDTO>();
            CreateMap<KhuyenMaiCreateDTO, KhuyenMai>();
            CreateMap<KhuyenMaiUpdateDTO, KhuyenMai>();

            // DiemThanhVien mappings
            CreateMap<DiemThanhVien, DiemThanhVienDTO>()
                .ForMember(dest => dest.TenKhachHang, opt => opt.MapFrom(src => src.KhachHang.HoTen))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.KhachHang.Email));

            // LichSuDiem mappings
            CreateMap<LichSuDiem, LichSuDiemDTO>();
        }
    }
}
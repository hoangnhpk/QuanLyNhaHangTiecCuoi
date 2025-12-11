using System.ComponentModel.DataAnnotations;

namespace QuanLyNhaHang.Models.ViewModels
{
    public class DatTiecVM
    {
        // --- Thông tin Khách hàng ---
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string SDT { get; set; }

        public string? Email { get; set; }

        // --- Thông tin Tiệc ---
        [Required(ErrorMessage = "Vui lòng nhập tên Cô dâu")]
        public string TenCoDau { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên Chú rể")]
        public string TenChuRe { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày tổ chức")]
        public DateTime? NgayToChuc { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng bàn")]
        public string? LoaiThucDon { get; set; }
        public string? LoaiDichVu { get; set; }
        public int? SoBanDat { get; set; }

        public int? SoBanDuPhong { get; set; }
        public string? ThucDonTuChonJson { get; set; }
        public string? DichVuTuChonJson { get; set; }

        public string? GhiChu { get; set; } // Dùng để lưu tạm các yêu cầu khác
        public TimeSpan? GioToChuc { get; set; } // Giờ

        public decimal? GiaBan { get; set; } // Giá món ăn/bàn
        public decimal? TienCoc { get; set; } // Tiền cọc
                                              // Tổng tiền không cần lưu vào bảng DatTiec (vì nó tính toán được), 
                                              // nhưng nếu muốn lưu vào PhieuThanhToan thì thêm vào sau.
    }
}
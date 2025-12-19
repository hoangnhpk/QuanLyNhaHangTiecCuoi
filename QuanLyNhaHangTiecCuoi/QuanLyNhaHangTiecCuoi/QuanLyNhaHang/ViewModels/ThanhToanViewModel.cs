using System;

namespace QuanLyNhaHang.ViewModels
{
    // Dùng cho danh sách hiển thị (Table)
    public class ThanhToanViewModel
    {
        public string MaDatTiec { get; set; }      // Key chính để định danh
        public string MaThanhToan { get; set; }    // MaPhieu (có thể null nếu chưa thanh toán hết)
        public string MaKhachHang { get; set; }
        public string TenKhachHang { get; set; }   // Để hiển thị tên KH nếu cần
        public decimal TongGiaTri { get; set; }    // Tổng tiền tiệc
        public decimal TienCoc { get; set; }       // 30% giá trị
        public string TrangThai { get; set; }      // "Đã cọc 30%" hoặc "Thanh toán hoàn toàn"
        public DateTime? NgayDatCoc { get; set; }
        public DateTime? NgayThanhToan { get; set; } // Ngày thanh toán nốt phần còn lại
        public bool IsCompleted { get; set; }      // Cờ để front-end đổi icon (Tích V hoặc Mắt)
    }

    // Dùng cho chi tiết Modal (Detail/Update)
    public class ThanhToanDetailModel
    {
        public string MaDatTiec { get; set; }
        public string MaThanhToan { get; set; }    // Nếu chưa có thì frontend tự generate hiển thị
        public string MaKhachHang { get; set; }
        public decimal TongGiaTri { get; set; }
        public decimal TienCoc { get; set; }       // Giá trị cọc 30%
        public decimal ConLai { get; set; }        // Số tiền cần thanh toán nốt
        public string TrangThai { get; set; }
        public DateTime? NgayDatCoc { get; set; }
        public DateTime? NgayThanhToan { get; set; }
        public string PhuongThucThanhToan { get; set; }
        public string GhiChu { get; set; }
    }

    // Dùng để lọc dữ liệu
    public class ThanhToanFilterModel
    {
        public string TrangThai { get; set; } // "ALL", "DEPOSIT", "COMPLETED"
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
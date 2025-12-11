
using System.ComponentModel.DataAnnotations;

namespace QuanLyNhaHang.ViewModels
{
    public class KhachHangEditViewModel
    {
        [Display(Name = "Mã khách hàng")]
        public string MaKhachHang { get; set; }

        [Display(Name = "Mã tài khoản")]
        public string? MaTaiKhoan { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên khách hàng.")]
        [StringLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự.")]
        [Display(Name = "Tên khách hàng")]
        [RegularExpression(@"^[^\d]*$", ErrorMessage = "Tên khách hàng không được chứa ký tự số.")]
        public string? TenKhachHang { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số CCCD/CMND.")]
        [StringLength(15, ErrorMessage = "CCCD/CMND không được vượt quá 15 ký tự.")]
        [Display(Name = "CCCD/CMND")]
        [RegularExpression(@"^\d+$", ErrorMessage = "CCCD/CMND chỉ được chứa ký tự số.")]
        public string? CccdKhachHang { get; set; }

        [Display(Name = "SĐT")]
        public string? SdtKhachHang { get; set; } // Chỉ hiển thị, không cho sửa

        [StringLength(255, ErrorMessage = "Địa chỉ không được vượt quá 255 ký tự.")]
        [Display(Name = "Địa chỉ")]
        public string? DiaChiKhachHang { get; set; }

        [StringLength(30)]
        [Display(Name = "Trạng thái")]
        public string? TrangThaiKhachHang { get; set; }

        [Display(Name = "Ghi chú")]
        public string? GhiChu { get; set; }

        // Thêm trường để lưu thông tin email từ tài khoản (chỉ hiển thị)
        [Display(Name = "Email")]
        public string? Email { get; set; }
    }
}
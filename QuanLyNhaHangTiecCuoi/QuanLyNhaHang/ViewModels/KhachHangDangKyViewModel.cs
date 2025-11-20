using System.ComponentModel.DataAnnotations;

namespace QuanLyNhaHang.ViewModels
{
    public class KhachHangDangKyViewModel
    {
        [Required]
        [Display(Name = "Tài khoản")]
        public string TaiKhoanKhachHang { get; set; }

        [Required]
        [Display(Name = "Mật khẩu")]
        public string MatKhauKhachHang { get; set; }

        [Required]
        [Display(Name = "Họ và tên")]
        public string TenKhachHang { get; set; }

        [Required]
        [Display(Name = "Số CCCD / CMND")]
        public string CccdKhachHang { get; set; }

        [Required]
        public string DiaChiKhachHang { get; set; }

        [Required]
        [EmailAddress]
        public string EmailKhachHang { get; set; }

        [Required]
        [Phone]
        public string SdtKhachHang { get; set; }

        [Required]
        [Compare("MatKhauKhachHang", ErrorMessage = "Mật khẩu không khớp")]
        public string NhapLaiMatKhau { get; set; }
    }
}

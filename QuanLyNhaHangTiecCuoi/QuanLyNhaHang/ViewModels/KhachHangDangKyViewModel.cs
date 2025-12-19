using System.ComponentModel.DataAnnotations;

namespace QuanLyNhaHang.ViewModels
{
    public class KhachHangDangKyViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tài khoản")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Tài khoản phải từ 5–20 ký tự")]
        public string TaiKhoanKhachHang { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [StringLength(12, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6–12 ký tự")]
        [DataType(DataType.Password)]
        public string MatKhauKhachHang { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ và tên")]
        [RegularExpression(@"^[\p{L}\s]+$", ErrorMessage = "Họ tên chỉ được chứa chữ cái và khoảng trắng")]
        public string TenKhachHang { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số CCCD/CMND")]
        [RegularExpression(@"^\d{9,12}$", ErrorMessage = "CCCD/CMND phải gồm 9–12 chữ số")]
        public string CccdKhachHang { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        public string DiaChiKhachHang { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Định dạng email không hợp lệ")]
        public string EmailKhachHang { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Số điện thoại phải gồm 10–11 chữ số")]
        public string SdtKhachHang { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập lại mật khẩu")]
        [Compare("MatKhauKhachHang", ErrorMessage = "Mật khẩu nhập lại không khớp")]
        [DataType(DataType.Password)]
        public string NhapLaiMatKhau { get; set; }
    }
}
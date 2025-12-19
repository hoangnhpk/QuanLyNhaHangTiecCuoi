using System.ComponentModel.DataAnnotations;

namespace QuanLyNhaHang.ViewModels
{
    public class KhachHangDangKyViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tài khoản")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Tài khoản phải từ 5–20 ký tự")]
        [Display(Name = "Tài khoản")]
        public string TaiKhoanKhachHang { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [StringLength(12, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6–12 ký tự")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string MatKhauKhachHang { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ và tên")]
        [RegularExpression(@"^[\p{L}\s]+$", ErrorMessage = "Họ tên chỉ được chứa chữ cái và khoảng trắng")]
        [StringLength(100, ErrorMessage = "Họ tên tối đa 100 ký tự")]
        [Display(Name = "Họ và tên")]
        public string TenKhachHang { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số CCCD/CMND")]
        [RegularExpression(@"^\d{9,12}$", ErrorMessage = "CCCD/CMND phải gồm 9–12 chữ số")]
        [Display(Name = "Số CCCD / CMND")]
        public string CccdKhachHang { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        [RegularExpression(@"^[\p{L}\d\s,.-]+$", ErrorMessage = "Địa chỉ không được chứa ký tự đặc biệt")]
        [StringLength(200, ErrorMessage = "Địa chỉ tối đa 200 ký tự")]
        [Display(Name = "Địa chỉ")]
        public string DiaChiKhachHang { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string EmailKhachHang { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Số điện thoại phải gồm 10–11 chữ số và chỉ chứa số")]
        [Display(Name = "Số điện thoại")]
        public string SdtKhachHang { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập lại mật khẩu")]
        [Compare("MatKhauKhachHang", ErrorMessage = "Mật khẩu nhập lại không khớp")]
        [DataType(DataType.Password)]
        [Display(Name = "Nhập lại mật khẩu")]
        public string NhapLaiMatKhau { get; set; }
    }
}

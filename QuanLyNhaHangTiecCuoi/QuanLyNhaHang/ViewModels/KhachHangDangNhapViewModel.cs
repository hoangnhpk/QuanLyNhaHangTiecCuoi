using System.ComponentModel.DataAnnotations;

namespace QuanLyNhaHang.ViewModels
{
    public class KhachHangDangNhapViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tài khoản hoặc email")]
        [Display(Name = "Tài khoản / Email")]
        public string TaiKhoan { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string MatKhau { get; set; }
    }
}

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaHang.Models
{
    [Table("TAI_KHOAN")]
    public class TaiKhoan
    {
        [Key]
        [StringLength(20, ErrorMessage = "Mã tài khoản không được vượt quá 20 ký tự.")]
        [DisplayName("Mã Tài Khoản")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Mã tài khoản chỉ được chứa chữ và số, không có ký tự đặc biệt.")]
        [Required(ErrorMessage = "Vui lòng nhập mã tài khoản.")]
        public string MaTaiKhoan { get; set; }

        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự.")]
        [DisplayName("Địa chỉ Email")]
        [EmailAddress(ErrorMessage = "Định dạng Email không hợp lệ.")]
        [Required(ErrorMessage = "Vui lòng nhập Email.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Tên đăng nhập phải từ 3 đến 30 ký tự.")]
        [DisplayName("Tên Đăng Nhập")]
        public string? UserName { get; set; }

        [StringLength(30, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 đến 30 ký tự.")]
        [DisplayName("Mật Khẩu")]
       
        [DataType(DataType.Password)] 
        public string? Password { get; set; }

        [StringLength(30)]
        [DisplayName("Vai Trò")]
        public string? VaiTro { get; set; }

        [StringLength(30)]
        [DisplayName("Trạng Thái")]
        [Required(ErrorMessage = "Vui lòng nhập trạng thái.")]
        public string? TrangThai { get; set; }
    }
}
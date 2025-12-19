using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaHang.Models
{
    [Table("QUAN_TRI_VIEN")]
    public class QuanTriVien
    {
        [Key]
        [StringLength(20, ErrorMessage = "Mã quản trị viên không được quá 20 ký tự.")]
        [Required(ErrorMessage = "Vui lòng nhập mã quản trị viên.")]
        [Display(Name = "Mã QTV")]
        public string MaQuanTriVien { get; set; }

        [StringLength(20)]
        [Display(Name = "Mã Tài Khoản")]
        public string? MaTaiKhoan { get; set; }

        [StringLength(100, ErrorMessage = "Tên không được quá 100 ký tự.")]
        [Required(ErrorMessage = "Vui lòng nhập họ và tên.")]
        [Display(Name = "Họ và Tên")]
        public string? TenQuanTriVien { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập cccd.")]
        [StringLength(12, MinimumLength = 9, ErrorMessage = "CCCD phải từ 9 đến 12 số.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "CCCD chỉ được chứa số.")]
        [Display(Name = "CCCD")]
        public string? Cccd { get; set; }

        [StringLength(15)]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
        [RegularExpression(@"^0[0-9]{9,10}$", ErrorMessage = "Số điện thoại không hợp lệ (phải bắt đầu bằng số 0 và có 10-11 số).")]
        [Display(Name = "Số điện thoại")]
        public string? SdtNV { get; set; }

        [StringLength(255, ErrorMessage = "Địa chỉ không được quá 255 ký tự.")]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ.")]
        [Display(Name = "Địa chỉ")]
        public string? DiaChi { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [EmailAddress(ErrorMessage = "Định dạng Email không hợp lệ.")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [StringLength(30)]
        [Display(Name = "Trạng thái")]
        public string? TrangThai { get; set; }

        [Display(Name = "Ghi chú")]
        public string? GhiChu { get; set; }

        // Navigation Property
        [ForeignKey("MaTaiKhoan")]
        public virtual TaiKhoan? TaiKhoan { get; set; }
    }
}
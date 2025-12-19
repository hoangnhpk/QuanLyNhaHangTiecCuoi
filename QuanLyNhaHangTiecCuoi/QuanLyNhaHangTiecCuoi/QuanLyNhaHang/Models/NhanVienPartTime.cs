using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaHang.Models
{
    [Table("NHAN_VIEN_PART_TIME")]
    public class NhanVienPartTime
    {
        [Key]
        [StringLength(20, ErrorMessage = "Mã quá dài.")]
        [Required(ErrorMessage = "Vui lòng nhập mã nhân viên.")]
        [Display(Name = "Mã NV PT")]
        public string MaNhanVienPT { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Vui lòng nhập tên nhân viên.")]
        [Display(Name = "Họ và Tên")]
        public string? TenNhanVienPT { get; set; }

        [StringLength(15)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "CCCD chỉ được chứa số.")]
        [Required(ErrorMessage = "Vui lòng nhập CCCD.")]
        [Display(Name = "CCCD")]
        public string? CccdNVPT { get; set; }

        [StringLength(15)]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
        [RegularExpression(@"^0[0-9]{9,10}$", ErrorMessage = "SĐT không hợp lệ (Bắt đầu bằng 0, dài 10-11 số).")]
        [Display(Name = "Số Điện Thoại")]
        public string? SdtNVPT { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ.")]
        [Display(Name = "Địa Chỉ")]
        public string? DiaChiNVPT { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng.")]
        [Display(Name = "Email")]
        public string? EmailNVPT { get; set; }

        [StringLength(30)]
        [Display(Name = "Trạng Thái")]
        public string? TrangThaiNV { get; set; }

        [Display(Name = "Ghi Chú")]
        public string? GhiChu { get; set; }

        public virtual ICollection<TT_SuDungNhanVien>? TT_SuDungNhanViens { get; set; }
    }
}
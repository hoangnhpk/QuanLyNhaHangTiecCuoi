using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaHang.Models
{
    [Table("NHAN_VIEN")]
    public class NhanVien
    {
        [Key]
        [StringLength(20, ErrorMessage = "Mã nhân viên tối đa 20 ký tự.")]
        [Required(ErrorMessage = "Vui lòng nhập mã nhân viên.")]
        [Display(Name = "Mã Nhân Viên")]
        public string MaNhanVien { get; set; }

        [StringLength(20)]
        [Required(ErrorMessage = "Vui lòng nhập mã bộ phận.")]
        [Display(Name = "Mã Bộ Phận")]
        public string? MaBoPhan { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Vui lòng nhập tên nhân viên.")]
        [Display(Name = "Họ và Tên")]
        public string? TenNhanVien { get; set; }

        [StringLength(15)]
        [Required(ErrorMessage = "Vui lòng nhập CCCD.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "CCCD chỉ được chứa số.")]
        [Display(Name = "CCCD")]
        public string? CccdNV { get; set; }

        [StringLength(15)]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
        [RegularExpression(@"^0[0-9]{9,10}$", ErrorMessage = "SĐT không hợp lệ (Bắt đầu bằng 0, dài 10-11 số).")]
        [Display(Name = "Số Điện Thoại")]
        public string? SdtNV { get; set; }

        [StringLength(255)]
        [Display(Name = "Địa Chỉ")]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ.")]
        public string? DiaChiNV { get; set; }

        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng.")]
        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [Display(Name = "Email")]
        public string? MailNV { get; set; }

        [StringLength(30)]
        [Display(Name = "Chức Vụ")]
        [Required(ErrorMessage = "Vui lòng nhập chức vụ nhân viên.")]
        public string? ChucVuNV { get; set; }

        [StringLength(30)]
        [Display(Name = "Trạng Thái")]
        public string? TrangThaiNV { get; set; }

        [Display(Name = "Ghi Chú")]
        public string? GhiChu { get; set; }

        [ForeignKey("MaBoPhan")]
        public virtual BoPhan? BoPhan { get; set; }
        public virtual ICollection<TT_SuDungNhanVien>? TT_SuDungNhanViens { get; set; }
    }
}
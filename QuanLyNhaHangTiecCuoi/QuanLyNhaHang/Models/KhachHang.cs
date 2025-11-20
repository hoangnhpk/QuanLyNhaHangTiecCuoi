using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaHang.Models
{
    [Table("KHACH_HANG")]
    public class KhachHang
    {
        public KhachHang()
        {
            // Khởi tạo ICollection để tránh lỗi null
            DatTiecs = new HashSet<DatTiec>();
        }

        [Key]
        [StringLength(20)]
        [Display(Name = "Mã khách hàng")]
        public string MaKhachHang { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên khách hàng.")]
        [StringLength(100)]
        [Display(Name = "Tên khách hàng")]
        [RegularExpression(@"^[^\d]*$", ErrorMessage = "Tên khách hàng không được chứa ký tự số.")]
        public string? TenKhachHang { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số CCCD/CMND.")]
        [StringLength(15)]
        [Display(Name = "CCCD/CMND")]
        [RegularExpression(@"^\d+$", ErrorMessage = "CCCD/CMND chỉ được chứa ký tự số.")]
        public string? CccdKhachHang { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        [StringLength(15)]
        [Display(Name = "SĐT")]
        public string? SdtKhachHang { get; set; }

        [StringLength(255)]
        [Display(Name = "Địa chỉ")]
        public string? DiaChiKhachHang { get; set; }

        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        [StringLength(100)]
        [Display(Name = "Email")]
        public string? EmailKhachHang { get; set; }

        [Required(ErrorMessage = "Tài khoản đăng nhập là bắt buộc.")]
        [StringLength(30)]
        [Display(Name = "Tài khoản")]
        public string? TaiKhoanKhachHang { get; set; }

        // Lưu ý: PHẢI HASH mật khẩu trước khi lưu vào DB!
      
        [StringLength(30)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string? MatKhauKhachHang { get; set; }

        [StringLength(30)]
        [Display(Name = "Trạng thái")]
        public string? TrangThaiKhachHang { get; set; }

        [Display(Name = "Ghi chú")]
        public string? GhiChu { get; set; }

        // Danh sách tiệc khách đã đặt
        public virtual ICollection<DatTiec> DatTiecs { get; set; }
    }
}
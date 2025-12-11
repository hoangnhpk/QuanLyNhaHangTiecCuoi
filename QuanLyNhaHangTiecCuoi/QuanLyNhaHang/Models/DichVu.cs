using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaHang.Models
{
    [Table("DICH_VU")]
    public class DichVu
    {
        public DichVu()
        {
            // KHỞI TẠO collection để tránh lỗi required
            TT_SuDungDichVus = new HashSet<TT_SuDungDichVu>();
        }

        [Key]
        [Required(ErrorMessage = "Mã dịch vụ là bắt buộc")]
        [StringLength(20, ErrorMessage = "Mã dịch vụ không được vượt quá 20 ký tự")]
        [Display(Name = "Mã Dịch Vụ")]
        public string MaDichVu { get; set; }

        [Required(ErrorMessage = "Tên dịch vụ là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên dịch vụ không được vượt quá 100 ký tự")]
        [Display(Name = "Tên Dịch Vụ")]
        public string TenDichVu { get; set; }

        [Required(ErrorMessage = "Giá dịch vụ là bắt buộc")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá dịch vụ phải lớn hơn hoặc bằng 0")]
        [Display(Name = "Giá Dịch Vụ")]
        public decimal GiaDV { get; set; }

        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
        [Display(Name = "Mô Tả Dịch Vụ")]
        public string? MoTaDV { get; set; }

        [StringLength(30, ErrorMessage = "Trạng thái không được vượt quá 30 ký tự")]
        [Display(Name = "Trạng Thái")]
        public string? TrangThaiDV { get; set; } = "Hoạt động";

        [StringLength(200, ErrorMessage = "Ghi chú không được vượt quá 200 ký tự")]
        [Display(Name = "Ghi Chú")]
        public string? GhiChu { get; set; }

        [Display(Name = "Hình Ảnh Dịch Vụ")]
        public string? HinhAnhDichVu { get; set; }

        // LOẠI BỎ validation cho collection này
        public virtual ICollection<TT_SuDungDichVu> TT_SuDungDichVus { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaHang.Models
{
    [Table("KHACH_HANG")]
    public class KhachHang
    {
        [Key]
        [StringLength(20)]
        [Display(Name = "Mã khách hàng")]
        public string MaKhachHang { get; set; } // Ví dụ: KH001

        [StringLength(100)]
        public string? TenKhachHang { get; set; }

        [StringLength(15)]
        public string? CccdKhachHang { get; set; }

        [StringLength(15)]
        public string? SdtKhachHang { get; set; }

        [StringLength(255)]
        public string? DiaChiKhachHang { get; set; }

        [StringLength(100)]
        public string? EmailKhachHang { get; set; }

        [StringLength(30)]
        public string? TaiKhoanKhachHang { get; set; }

        [StringLength(30)]
        public string? MatKhauKhachHang { get; set; }

        [StringLength(30)] // Đã sửa thành NVARCHAR(30)
        public string? TrangThaiKhachHang { get; set; }

        public string? GhiChu { get; set; }

        // Danh sách tiệc khách đã đặt
        public virtual ICollection<DatTiec> DatTiecs { get; set; }
    }
}
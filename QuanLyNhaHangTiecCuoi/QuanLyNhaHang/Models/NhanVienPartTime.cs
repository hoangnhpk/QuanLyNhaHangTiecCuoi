using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaHang.Models
{
    [Table("NHAN_VIEN_PART_TIME")]
    public class NhanVienPartTime
    {
        [Key]
        [StringLength(20)]
        public string MaNhanVienPT { get; set; }

        [StringLength(100)]
        public string? TenNhanVienPT { get; set; }

        [StringLength(15)]
        public string? CccdNVPT { get; set; }

        [StringLength(15)]
        public string? SdtNVPT { get; set; }

        [StringLength(255)]
        public string? DiaChiNVPT { get; set; }

        [StringLength(100)]
        public string? EmailNVPT { get; set; }

        [StringLength(30)]
        public string? TrangThaiNV { get; set; }

        public string? GhiChu { get; set; }

        // Thêm danh sách này để biết nhân viên này đã làm những tiệc nào
        public virtual ICollection<TT_SuDungNhanVien> TT_SuDungNhanViens { get; set; }
    }
}
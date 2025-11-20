using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaHang.Models
{
    [Table("NHAN_VIEN")]
    public class NhanVien
    {
        [Key]
        [StringLength(20)]
        public string MaNhanVien { get; set; } // Ví dụ: NV001

        [StringLength(20)]
        public string MaBoPhan { get; set; } // Khóa ngoại string

        [StringLength(100)]
        public string? TenNhanVien { get; set; }

        [StringLength(15)]
        public string? CccdNV { get; set; }

        [StringLength(15)]
        public string? SdtNV { get; set; }

        [StringLength(255)]
        public string? DiaChiNV { get; set; }

        [StringLength(100)]
        public string? MailNV { get; set; }

        [StringLength(30)]
        public string? TaiKhoan { get; set; }

        [StringLength(30)]
        public string? MatKhau { get; set; }
        [StringLength(30)]
        public string? ChucVuNV { get; set; }

        [StringLength(30)]
        public string? TrangThaiNV { get; set; }

        public string? GhiChu { get; set; }

        [ForeignKey("MaBoPhan")]
        public virtual BoPhan BoPhan { get; set; }
    }
}
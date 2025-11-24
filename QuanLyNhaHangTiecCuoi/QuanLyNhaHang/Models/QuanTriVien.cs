using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaHang.Models
{
    [Table("QUAN_TRI_VIEN")]
    public class QuanTriVien
    {
        [Key]
        [StringLength(20)]
        public string MaQuanTriVien { get; set; }

        [StringLength(20)]
        public string? MaTaiKhoan { get; set; }

        [StringLength(100)]
        public string? TenQuanTriVien { get; set; }

        [StringLength(15)]
        public string? Cccd { get; set; }

        [StringLength(15)]
        public string? SdtNV { get; set; }

        [StringLength(255)]
        public string? DiaChi { get; set; }

        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(30)]
        public string? TrangThai { get; set; }

        public string? GhiChu { get; set; }
        [ForeignKey("MaTaiKhoan")]
        public virtual TaiKhoan TaiKhoan { get; set; }
    }
}
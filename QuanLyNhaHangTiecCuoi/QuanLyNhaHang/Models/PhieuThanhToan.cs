using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaHang.Models
{
    [Table("PHIEU_THANH_TOAN")]
    public class PhieuThanhToan
    {
        [Key]
        [StringLength(20)]
        public string MaPhieu { get; set; }

        [StringLength(20)]
        public string MaDatTiec { get; set; } // Khóa ngoại 1-1

        public DateTime? NgayThanhToan { get; set; }

        [StringLength(50)]
        public string PhuongThucThanhToan { get; set; }

        public decimal? TongTien { get; set; }

        [StringLength(30)]
        public string TrangThai { get; set; }

        public string GhiChu { get; set; }

        [ForeignKey("MaDatTiec")]
        public virtual DatTiec DatTiec { get; set; }
    }
}
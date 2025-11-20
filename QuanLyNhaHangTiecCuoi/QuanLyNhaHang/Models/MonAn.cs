using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaHang.Models
{
    [Table("MON_AN")]
    public class MonAn
    {
        [Key]
        [StringLength(20)]
        public string MaMonAn { get; set; } // Ví dụ: MA001

        [StringLength(100)]
        public string? TenMonAn { get; set; }

        [StringLength(20)]
        public string? DonViTinh { get; set; }

        public decimal? DonGia { get; set; }

        [StringLength(500)]
        public string? MoTaMonAn { get; set; }

        [StringLength(50)]
        public string? LoaiMonAn { get; set; }

        [StringLength(30)]
        public string? TrangThaiMonAn { get; set; }

        public string? GhiChu { get; set; }
        public string? HinhAnhMonAn { get; set; }

        public virtual ICollection<ChiTietThucDon> ChiTietThucDons { get; set; }
    }
}
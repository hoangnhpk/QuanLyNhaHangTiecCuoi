using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaHang.Models
{
    [Table("TT_SU_DUNG_DICH_VU")]
    public class TT_SuDungDichVu
    {
        [Key]
        [StringLength(20)]
        public string MaThongTinDV { get; set; }

        [StringLength(20)]
        public string? MaDatTiec { get; set; }

        [StringLength(20)]
        public string? MaDichVu { get; set; }

        public int? SoLuong { get; set; }

        public DateTime? NgaySuDung { get; set; }

        public string? GhiChu { get; set; }

        [ForeignKey("MaDatTiec")]
        public virtual DatTiec DatTiec { get; set; }

        [ForeignKey("MaDichVu")]
        public virtual DichVu DichVu { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaHang.Models
{
    [Table("TT_SU_DUNG_NHAN_VIEN")]
    public class TT_SuDungNhanVien
    {
        [Key]
        [StringLength(20)]
        public string MaThongTinNV { get; set; }

        [StringLength(20)]
        public string? MaDatTiec { get; set; }

        [StringLength(20)]
        public string? MaNhanVien { get; set; }

        public string? GhiChu { get; set; }

        [ForeignKey("MaDatTiec")]
        public virtual DatTiec DatTiec { get; set; }

        [ForeignKey("MaNhanVien")]
        public virtual NhanVien NhanVien { get; set; }
    }
}
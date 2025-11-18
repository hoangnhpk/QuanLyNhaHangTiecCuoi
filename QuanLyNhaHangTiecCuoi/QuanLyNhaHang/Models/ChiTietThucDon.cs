using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaHang.Models
{
    [Table("CHI_TIET_THUC_DON")]
    public class ChiTietThucDon
    {
        [Key]
        [StringLength(20)]
        public string MaChiTietThucDon { get; set; }

        [StringLength(20)]
        public string MaDatTiec { get; set; }

        [StringLength(20)]
        public string MaMonAn { get; set; }

        public int? SoLuongMotBan { get; set; }

        [StringLength(255)]
        public string GhiChuThem { get; set; }

        [ForeignKey("MaDatTiec")]
        public virtual DatTiec DatTiec { get; set; }

        [ForeignKey("MaMonAn")]
        public virtual MonAn MonAn { get; set; }
    }
}
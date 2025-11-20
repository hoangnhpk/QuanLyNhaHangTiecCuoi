using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaHang.Models
{
    [Table("CHI_TIET_COMBO")]
    public class ChiTietCombo
    {
        [Key]
        [StringLength(20)]
        public string MaChiTietCombo { get; set; }

        [StringLength(255)] 
        public string? GhiChu { get; set; }

        public int? SoLuong { get; set; }

       

        [StringLength(20)]
        public string? MaComboMon { get; set; } 

        [ForeignKey("MaComboMon")]
        public virtual ComboMon ComboMon { get; set; }

        [StringLength(20)]
        public string? MaMonAn { get; set; } 

        [ForeignKey("MaMonAn")]
        public virtual MonAn MonAn { get; set; }
    }
}
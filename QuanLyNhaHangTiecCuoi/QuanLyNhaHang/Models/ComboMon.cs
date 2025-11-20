using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaHang.Models
{
    [Table("COMBO_MON")]
    public class ComboMon
    {
        [Key]
        [StringLength(20)]
        public string MaComboMon { get; set; }

        [StringLength(100)]
        public string? TenCombo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayTaoCombo { get; set; }

        public int? SoLuong { get; set; }

        public decimal? GiaCombo { get; set; }

        [StringLength(30)]
        public string? TrangThai { get; set; }

        public string? MoTa { get; set; }
        public string? HinhAnhCombo { get; set; }

        public virtual ICollection<ChiTietCombo> ChiTietCombos { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaHang.Models
{
    [Table("COMBO_MON")]
    public class ComboMon
    {
        [Key]
        [StringLength(20)]
        [Required(ErrorMessage = "Mã Combo không được để trống")]
        public string MaComboMon { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Tên Combo không được để trống")]
        public string? TenCombo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayTaoCombo { get; set; }

        [Range(1, 1000, ErrorMessage = "Số lượng phải từ 1 đến 1000")]
        [Required(ErrorMessage = "Số lượng không được để trống")]
        public int? SoLuong { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá bán không được âm")]
        [Required(ErrorMessage = "Giá bán không được để trống")]
        public decimal? GiaCombo { get; set; }

        [StringLength(30)]
        public string? TrangThai { get; set; }
        [Required(ErrorMessage = "Mô tả Combo không được để trống")]
        public string? MoTa { get; set; }
        public string? HinhAnhCombo { get; set; }

        public virtual ICollection<ChiTietCombo> ChiTietCombos { get; set; }
    }
}
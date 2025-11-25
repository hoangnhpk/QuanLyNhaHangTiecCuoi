using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaHang.Models
{
    [Table("MON_AN")]
    public class MonAn
    {
        [Key]
        [Required(ErrorMessage = "Mã món ăn không được để trống")]
        [StringLength(20)]
        [Column("MaMonAn")]
        [Display(Name = "Mã món ăn")]
        public string MaMonAn { get; set; } = null!;

        [StringLength(100)]
        [Required(ErrorMessage = "Tên món ăn không được để trống")]
        [Display(Name = "Tên món ăn")]
        public string? TenMonAn { get; set; }

        [StringLength(20)]
        [Required(ErrorMessage = "Đơn vị tính món ăn không được để trống")]
        [Display(Name = "Đơn vị tính")]
        public string? DonViTinh { get; set; }


        [Required(ErrorMessage = "Đơn giá món ăn không được để trống")]

        [Display(Name = "Đơn giá")]
        [Range(0, double.MaxValue, ErrorMessage = "Đơn giá phải lớn hơn hoặc bằng 0")]
        public decimal? DonGia { get; set; }

        [StringLength(500)]
        [Required(ErrorMessage = "Mô tả món ăn không được để trống")]
        [Display(Name = "Mô tả món ăn")]
        public string? MoTaMonAn { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Loại món ăn không được để trống")]
        [Display(Name = "Loại món ăn")]
        public string? LoaiMonAn { get; set; }

        [StringLength(30)]
        [Column("TrangThaiMonAn")]
        [Display(Name = "Trạng thái món ăn")]
        public string? TrangThaiMonAn { get; set; }

        [Column("GhiChu")]
        [Display(Name = "Ghi chú")]
        public string? GhiChu { get; set; }

        public virtual ICollection<ChiTietThucDon>? ChiTietThucDons { get; set; }

        public string? HinhAnhMonAn { get; set; }

        

    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaHang.Models
{
    [Table("HINH_ANH")]
    public class HinhAnh
    {
        [Key]
        [StringLength(20)]
        public string MaHinhAnh { get; set; }

        [StringLength(20)]
        public string? MaMonAn { get; set; } // Cho phép null (không cần Required)

        [StringLength(20)]
        public string? MaDichVu { get; set; } // Cho phép null

        public string? Url { get; set; }

        [StringLength(30)]
        public string? TrangThaiHinhAnh { get; set; }

        public string? GhiChu { get; set; }
        // Tạo mối quan hệ để EF Core hiểu và vẽ dây
        [ForeignKey("MaMonAn")]
        public virtual MonAn MonAn { get; set; }

        [ForeignKey("MaDichVu")]
        public virtual DichVu DichVu { get; set; }
    }
}
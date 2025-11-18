using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaHang.Models
{
    [Table("BO_PHAN")]
    public class BoPhan
    {
        [Key]
        [StringLength(20)]
        public string MaBoPhan { get; set; } // Ví dụ: BP001

        [StringLength(50)]
        public string TenBoPhan { get; set; }

        public decimal? TienCong { get; set; } // Dấu ? cho phép null

        [StringLength(30)]
        public string TrangThai { get; set; }

        public string GhiChu { get; set; }

        public virtual ICollection<NhanVien> NhanViens { get; set; }
    }
}
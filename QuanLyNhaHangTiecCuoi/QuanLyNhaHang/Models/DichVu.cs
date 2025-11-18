using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaHang.Models
{
    [Table("DICH_VU")]
    public class DichVu
    {
        [Key]
        [StringLength(20)]
        public string MaDichVu { get; set; } // Ví dụ: DV001

        [StringLength(100)]
        public string TenDichVu { get; set; }

        public decimal? GiaDV { get; set; }

        public string MoTaDV { get; set; }

        [StringLength(30)]
        public string TrangThaiDV { get; set; }

        public string GhiChu { get; set; }

        public virtual ICollection<TT_SuDungDichVu> TT_SuDungDichVus { get; set; }
    }
}
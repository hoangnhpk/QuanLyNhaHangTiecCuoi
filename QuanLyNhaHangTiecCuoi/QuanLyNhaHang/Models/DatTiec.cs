using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaHang.Models
{
    [Table("DAT_TIEC")]
    public class DatTiec
    {
        [Key]
        [StringLength(20)]
        public string MaDatTiec { get; set; } // Ví dụ: DT001

        [StringLength(20)]
        public string MaKhachHang { get; set; }

        [StringLength(100)]
        public string TenCoDau { get; set; }

        [StringLength(100)]
        public string TenChuRe { get; set; }

        public DateTime? NgayDatTiec { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayToChuc { get; set; }

        public TimeSpan? GioToChuc { get; set; } // Kiểu TIME

        public int? SoBanDat { get; set; }

        public decimal? GiaBan { get; set; }

        public decimal? TienCoc { get; set; }

        [StringLength(30)]
        public string TrangThai { get; set; }

        public string ChiTiet { get; set; }

        [ForeignKey("MaKhachHang")]
        public virtual KhachHang KhachHang { get; set; }

        public virtual ICollection<ChiTietThucDon> ChiTietThucDons { get; set; }
        public virtual ICollection<TT_SuDungDichVu> TT_SuDungDichVus { get; set; }
        public virtual ICollection<TT_SuDungNhanVien> TT_SuDungNhanViens { get; set; }
        public virtual PhieuThanhToan PhieuThanhToan { get; set; }
    }
}
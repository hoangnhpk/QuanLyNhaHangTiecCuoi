using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyNhaHang.Models
{
    [Table("TAI_KHOAN")]
    public class TaiKhoan
    {
        [Key]
        [StringLength(20)]
        public string MaTaiKhoan { get; set; }
        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(30)]
        public string? UserName { get; set; }

        [StringLength(30)]
        public string? Password { get; set; }
        [StringLength(30)]
        public string? VaiTro { get; set; }

        [StringLength(30)] 
        public string? TrangThai { get; set; }



    }
}
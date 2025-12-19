using System.ComponentModel.DataAnnotations;

namespace QuanLyNhaHang.Models
{
    public class PasswordResetToken
    {
        [Key]
        public string Token { get; set; }
        public string Email { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
}

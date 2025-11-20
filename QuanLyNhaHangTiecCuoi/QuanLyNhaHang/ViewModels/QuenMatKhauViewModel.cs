using System.ComponentModel.DataAnnotations;

namespace QuanLyNhaHang.ViewModels
{
    public class QuenMatKhauViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}

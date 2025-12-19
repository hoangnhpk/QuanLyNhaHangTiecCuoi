using System.ComponentModel.DataAnnotations;

namespace QuanLyNhaHang.ViewModels
{
    public class GuiEmailViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập Email người nhận")]
        [EmailAddress(ErrorMessage = "Định dạng Email không hợp lệ")]
        [Display(Name = "Gửi đến Email")]
        public string EmailNguoiNhan { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]

        [Display(Name = "Tiêu đề Email")]
        public string TieuDe { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập nội dung")]
        [Display(Name = "Nội dung")]
        [DataType(DataType.MultilineText)]
        public string NoiDung { get; set; }
    }
}
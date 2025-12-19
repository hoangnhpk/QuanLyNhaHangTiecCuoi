using System.ComponentModel.DataAnnotations;

namespace QuanLyNhaHang.ViewModels
{
    public class HoanThienGoogleVM
    {
        [Required(ErrorMessage = "Vui lòng nhập số CCCD")]
        [RegularExpression(@"^\d{9,12}$", ErrorMessage = "CCCD/CMND phải gồm 9–12 chữ số")]
        public string Cccd { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Số điện thoại phải từ 10-11 số")]
        public string Sdt { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        public string DiaChi { get; set; }
    }
}
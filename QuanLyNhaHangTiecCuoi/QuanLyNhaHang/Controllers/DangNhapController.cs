using Microsoft.AspNetCore.Mvc;
using QuanLyNhaHang.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace QuanLyNhaHang.Controllers
{
    public class DangNhapController : Controller
    {
        private readonly QuanLyNhaHangContext _context;

        public DangNhapController(QuanLyNhaHangContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string TaiKhoan, string MatKhau)
        {
            // Tìm tài khoản theo username hoặc email
            var taiKhoan = _context.TaiKhoans
                .FirstOrDefault(t => (t.UserName == TaiKhoan || t.Email == TaiKhoan) && t.Password == MatKhau);

            if (taiKhoan != null)
            {
                // Lấy thông tin khách hàng gắn với tài khoản
                var khach = _context.KhachHangs.FirstOrDefault(k => k.MaTaiKhoan == taiKhoan.MaTaiKhoan);

                if (khach != null)
                {
                    HttpContext.Session.SetString("MaKhachHang", khach.MaKhachHang);
                    HttpContext.Session.SetString("TenDangNhap", taiKhoan.UserName ?? "");
                    HttpContext.Session.SetString("TenKhachHang", khach.TenKhachHang ?? "");
                }

                return RedirectToAction("Index", "Home");
            }

            ViewBag.ThongBao = "Sai tài khoản hoặc mật khẩu!";
            return View();
        }

        public IActionResult DangXuat()
        {
            HttpContext.Session.Clear(); // Xóa toàn bộ session
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult QuenMatKhau()
        {
            return View();
        }

        [HttpPost]
        public IActionResult QuenMatKhau(string Email)
        {
            // Kiểm tra email trong bảng TAI_KHOAN
            var taiKhoan = _context.TaiKhoans.FirstOrDefault(t => t.Email == Email);
            if (taiKhoan == null)
            {
                ViewBag.ThongBao = "Email không tồn tại.";
                return View();
            }

            // Tạo token reset
            string token = Guid.NewGuid().ToString();
            var resetToken = new PasswordResetToken
            {
                Token = token,
                Email = Email,
                ExpiryTime = DateTime.Now.AddMinutes(30)
            };

            // Xóa token cũ nếu có
            var oldToken = _context.PasswordResetTokens.FirstOrDefault(t => t.Email == Email);
            if (oldToken != null)
            {
                _context.PasswordResetTokens.Remove(oldToken);
            }

            _context.PasswordResetTokens.Add(resetToken);
            _context.SaveChanges();

            // Tạo link reset
            string resetLink = Url.Action("DatLaiMatKhau", "DangNhap", new { token = token }, Request.Scheme);

            string subject = "Đặt lại mật khẩu";
            string body = $"Xin chào {taiKhoan.UserName},\n\nVui lòng nhấn vào liên kết sau để đặt lại mật khẩu:\n{resetLink}\n\nLiên kết sẽ hết hạn sau 30 phút.";

            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("aunxpk04299@gmail.com", "fqme nhhv ngbv rsed"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("aunxpk04299@gmail.com"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = false,
                };
                mailMessage.To.Add(Email);

                smtpClient.Send(mailMessage);
                ViewBag.ThongBao = "Liên kết đặt lại mật khẩu đã được gửi đến email.";
            }
            catch (Exception ex)
            {
                ViewBag.ThongBao = "Không thể gửi email. Vui lòng thử lại sau.";
                Console.WriteLine("Lỗi gửi email: " + ex.Message);
            }

            return View();
        }

        [HttpGet]
        public IActionResult DatLaiMatKhau(string token)
        {
            var resetToken = _context.PasswordResetTokens
                .FirstOrDefault(t => t.Token == token && t.ExpiryTime > DateTime.Now);

            if (resetToken == null)
            {
                return Content("Liên kết không hợp lệ hoặc đã hết hạn.");
            }

            ViewBag.Token = token;
            return View();
        }

        [HttpPost]
        public IActionResult DatLaiMatKhau(string token, string MatKhauMoi, string XacNhanMatKhauMoi)
        {
            if (MatKhauMoi != XacNhanMatKhauMoi)
            {
                ViewBag.ThongBao = "Mật khẩu xác nhận không khớp!";
                ViewBag.Token = token;
                return View();
            }

            var resetToken = _context.PasswordResetTokens
                .FirstOrDefault(t => t.Token == token && t.ExpiryTime > DateTime.Now);

            if (resetToken == null)
            {
                return Content("Liên kết không hợp lệ hoặc đã hết hạn.");
            }

            // Tìm tài khoản theo email
            var taiKhoan = _context.TaiKhoans.FirstOrDefault(t => t.Email == resetToken.Email);
            if (taiKhoan != null)
            {
                taiKhoan.Password = MatKhauMoi; // TODO: Hash mật khẩu trước khi lưu
                _context.PasswordResetTokens.Remove(resetToken);
                _context.SaveChanges();
                return RedirectToAction("Index", "DangNhap");
            }

            return Content("Không tìm thấy tài khoản.");
        }


    }
}

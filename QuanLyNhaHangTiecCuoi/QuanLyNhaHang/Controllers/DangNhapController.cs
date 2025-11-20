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
<<<<<<< HEAD
        //public IActionResult Index(string TaiKhoan, string MatKhau)
        //{
        //    var khach = _context.KhachHangs.FirstOrDefault(k => (k.TaiKhoanKhachHang == TaiKhoan || k.EmailKhachHang == TaiKhoan) && k.MatKhauKhachHang == MatKhau);
        //    if (khach != null)
        //    {
        //        HttpContext.Session.SetString("MaKhachHang", khach.MaKhachHang);
        //        HttpContext.Session.SetString("TenDangNhap", khach.TaiKhoanKhachHang);
        //        HttpContext.Session.SetString("TenKhachHang", khach.TenKhachHang ?? "");
        //        return RedirectToAction("Index", "Home");
        //    }

        //    ViewBag.ThongBao = "Sai tài khoản - email hoặc mật khẩu!";
        //    return View();
        //}
=======
        public IActionResult Index(string TaiKhoan, string MatKhau)
        {
            var khach = _context.KhachHangs.FirstOrDefault(k => (k.TaiKhoanKhachHang == TaiKhoan || k.EmailKhachHang == TaiKhoan) && k.MatKhauKhachHang == MatKhau);
            if (khach != null)
            {
                HttpContext.Session.SetString("MaKhachHang", khach.MaKhachHang);
                HttpContext.Session.SetString("TenDangNhap", khach.TaiKhoanKhachHang);
                HttpContext.Session.SetString("TenKhachHang", khach.TenKhachHang ?? "");
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ThongBao = "Sai tài khoản - email hoặc mật khẩu!";
            return View();
        }
>>>>>>> 03c721d578e498f3bfe8ffced87e4f47b8796137
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
<<<<<<< HEAD
        //public IActionResult QuenMatKhau(string Email)
        //{
        //    var khach = _context.KhachHangs.FirstOrDefault(k => k.EmailKhachHang == Email);
        //    if (khach == null)
        //    {
        //        ViewBag.ThongBao = "Email không tồn tại.";
        //        return View();
        //    }

        //    string token = Guid.NewGuid().ToString();
        //    var resetToken = new PasswordResetToken
        //    {
        //        Token = token,
        //        Email = Email,
        //        ExpiryTime = DateTime.Now.AddMinutes(30)
        //    };
        //    var oldToken = _context.PasswordResetTokens.FirstOrDefault(t => t.Email == Email);
        //    if (oldToken != null)
        //    {
        //        _context.PasswordResetTokens.Remove(oldToken);
        //    }
        //    _context.PasswordResetTokens.Add(resetToken);
        //    _context.SaveChanges();

        //    string resetLink = Url.Action("DatLaiMatKhau", "DangNhap", new { token = token }, Request.Scheme);

        //    string subject = "Đặt lại mật khẩu";
        //    string body = $"Xin chào {khach.TenKhachHang},\n\nVui lòng nhấn vào liên kết sau để đặt lại mật khẩu:\n{resetLink}\n\nLiên kết sẽ hết hạn sau 30 phút.";

        //    try
        //    {
        //        var smtpClient = new SmtpClient("smtp.gmail.com")
        //        {
        //            Port = 587,
        //            Credentials = new NetworkCredential("aunxpk04299@gmail.com", "fqme nhhv ngbv rsed"),
        //            EnableSsl = true,
        //        };

        //        var mailMessage = new MailMessage
        //        {
        //            From = new MailAddress("aunxpk04299@gmail.com"),
        //            Subject = subject,
        //            Body = body,
        //            IsBodyHtml = false,
        //        };
        //        mailMessage.To.Add(Email);

        //        smtpClient.Send(mailMessage);
        //        ViewBag.ThongBao = "Liên kết đặt lại mật khẩu đã được gửi đến email.";
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.ThongBao = "Không thể gửi email. Vui lòng thử lại sau.";
        //        Console.WriteLine("Lỗi gửi email: " + ex.Message);
        //    }


        //    return View();
        //}
=======
        public IActionResult QuenMatKhau(string Email)
        {
            var khach = _context.KhachHangs.FirstOrDefault(k => k.EmailKhachHang == Email);
            if (khach == null)
            {
                ViewBag.ThongBao = "Email không tồn tại.";
                return View();
            }

            string token = Guid.NewGuid().ToString();
            var resetToken = new PasswordResetToken
            {
                Token = token,
                Email = Email,
                ExpiryTime = DateTime.Now.AddMinutes(30)
            };
            var oldToken = _context.PasswordResetTokens.FirstOrDefault(t => t.Email == Email);
            if (oldToken != null)
            {
                _context.PasswordResetTokens.Remove(oldToken);
            }
            _context.PasswordResetTokens.Add(resetToken);
            _context.SaveChanges();

            string resetLink = Url.Action("DatLaiMatKhau", "DangNhap", new { token = token }, Request.Scheme);

            string subject = "Đặt lại mật khẩu";
            string body = $"Xin chào {khach.TenKhachHang},\n\nVui lòng nhấn vào liên kết sau để đặt lại mật khẩu:\n{resetLink}\n\nLiên kết sẽ hết hạn sau 30 phút.";

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
>>>>>>> 03c721d578e498f3bfe8ffced87e4f47b8796137

        [HttpGet]
        public IActionResult DatLaiMatKhau(string token)
        {
            var resetToken = _context.PasswordResetTokens.FirstOrDefault(t => t.Token == token && t.ExpiryTime > DateTime.Now);
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

            var resetToken = _context.PasswordResetTokens.FirstOrDefault(t => t.Token == token && t.ExpiryTime > DateTime.Now);
            if (resetToken == null)
            {
                return Content("Liên kết không hợp lệ hoặc đã hết hạn.");
            }

<<<<<<< HEAD
            //var khach = _context.KhachHangs.FirstOrDefault(k => k.EmailKhachHang == resetToken.Email);
            //if (khach != null)
            //{
            //    khach.MatKhauKhachHang = MatKhauMoi; // TODO: Hash mật khẩu trước khi lưu
            //    _context.PasswordResetTokens.Remove(resetToken);
            //    _context.SaveChanges();
            //    return RedirectToAction("Index", "DangNhap");
            //}
=======
            var khach = _context.KhachHangs.FirstOrDefault(k => k.EmailKhachHang == resetToken.Email);
            if (khach != null)
            {
                khach.MatKhauKhachHang = MatKhauMoi; // TODO: Hash mật khẩu trước khi lưu
                _context.PasswordResetTokens.Remove(resetToken);
                _context.SaveChanges();
                return RedirectToAction("Index", "DangNhap");
            }
>>>>>>> 03c721d578e498f3bfe8ffced87e4f47b8796137

            return Content("Không tìm thấy người dùng.");
        }

    }
}

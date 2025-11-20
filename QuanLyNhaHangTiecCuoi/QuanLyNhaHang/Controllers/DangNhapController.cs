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

>>>>>>> 5e50c9fd72bb9b65d498521c73fa2e7bc0642943
        public IActionResult DangXuat()
        {
            HttpContext.Session.Clear(); // Xóa toàn bộ session
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
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
            var taiKhoan = _context.TaiKhoans.FirstOrDefault(t => t.Email == Email);
            if (taiKhoan == null)
            {
                ViewBag.ThongBao = "Email không tồn tại.";
                return View();
            }

            // Tạo mã OTP
            string otp = new Random().Next(100000, 999999).ToString();

            // Lưu vào session
            HttpContext.Session.SetString("OTP_Reset", otp);
            HttpContext.Session.SetString("OTP_Email", Email);
            HttpContext.Session.SetString("OTP_Expiry", DateTime.Now.AddMinutes(10).ToString());

            // Gửi email
            string subject = "Mã OTP đặt lại mật khẩu";
            string body = $"Xin chào {taiKhoan.UserName},\n\nMã OTP để đặt lại mật khẩu của bạn là: {otp}\nMã có hiệu lực trong 10 phút.";

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
                return RedirectToAction("XacThucOtp");
            }
            catch (Exception ex)
            {
                ViewBag.ThongBao = "Không thể gửi email. Vui lòng thử lại sau.";
                Console.WriteLine("Lỗi gửi OTP: " + ex.Message);
                return View();
            }
        }
>>>>>>> 5e50c9fd72bb9b65d498521c73fa2e7bc0642943

        [HttpGet]
        public IActionResult DatLaiMatKhau()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DatLaiMatKhau(string MatKhauMoi, string XacNhanMatKhauMoi)
        {
            var email = HttpContext.Session.GetString("ResetEmail");
            if (string.IsNullOrEmpty(email))
            {
                return Content("Phiên không hợp lệ.");
            }

            if (MatKhauMoi != XacNhanMatKhauMoi)
            {
                ViewBag.ThongBao = "Mật khẩu xác nhận không khớp!";
                return View();
            }

            var taiKhoan = _context.TaiKhoans.FirstOrDefault(t => t.Email == email);
            if (taiKhoan != null)
            {
<<<<<<< HEAD
                return Content("Liên kết không hợp lệ hoặc đã hết hạn.");
            }


            //var khach = _context.KhachHangs.FirstOrDefault(k => k.EmailKhachHang == resetToken.Email);
            //if (khach != null)
            //{
            //    khach.MatKhauKhachHang = MatKhauMoi; // TODO: Hash mật khẩu trước khi lưu
            //    _context.PasswordResetTokens.Remove(resetToken);
            //    _context.SaveChanges();
            //    return RedirectToAction("Index", "DangNhap");
            //}

=======
                taiKhoan.Password = MatKhauMoi; // TODO: Hash mật khẩu
                _context.SaveChanges();

                HttpContext.Session.Remove("ResetEmail");
                return RedirectToAction("Index", "DangNhap");
            }
>>>>>>> 5e50c9fd72bb9b65d498521c73fa2e7bc0642943

            return Content("Không tìm thấy tài khoản.");
        }

        [HttpGet]
        public IActionResult XacThucOtp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult XacThucOtp(string MaOtp)
        {
            var otpSession = HttpContext.Session.GetString("OTP_Reset");
            var email = HttpContext.Session.GetString("OTP_Email");
            var expiryString = HttpContext.Session.GetString("OTP_Expiry");

            if (string.IsNullOrEmpty(otpSession) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(expiryString))
            {
                ViewBag.ThongBao = "Phiên OTP không hợp lệ. Vui lòng thử lại.";
                return View();
            }

            if (MaOtp != otpSession)
            {
                ViewBag.ThongBao = "Mã OTP không đúng.";
                return View();
            }

            if (DateTime.TryParse(expiryString, out DateTime expiry) && DateTime.Now > expiry)
            {
                ViewBag.ThongBao = "Mã OTP đã hết hạn.";
                return View();
            }

            // OTP hợp lệ → chuyển sang form đặt lại mật khẩu
            HttpContext.Session.SetString("ResetEmail", email);
            return RedirectToAction("DatLaiMatKhau");
        }



    }
}

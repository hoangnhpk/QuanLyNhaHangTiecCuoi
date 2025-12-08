using Microsoft.AspNetCore.Mvc;
using QuanLyNhaHang.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System;
using QuanLyNhaHang.Services;

namespace QuanLyNhaHang.Controllers
{
    public class DangNhapController : Controller
    {
        private readonly QuanLyNhaHangContext _context;
        private readonly IEmailService _emailService; // 1. Khai báo biến hứng Service
        // 2. Tiêm vào Constructor
        public DangNhapController(QuanLyNhaHangContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService; // Nhận hàng từ DI
        }

        public DangNhapController(QuanLyNhaHangContext context)
        {
            _context = context;
        }

        // --- TRANG ĐĂNG NHẬP ---
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string TaiKhoan, string MatKhau)
        {
            // 1. Tìm tài khoản (Email nằm ở đây)
            var taiKhoan = _context.TaiKhoans
                .FirstOrDefault(t => (t.UserName == TaiKhoan || t.Email == TaiKhoan) && t.Password == MatKhau);

            if (taiKhoan != null)
            {
                // 2. Tìm thông tin khách hàng (Tên, SĐT nằm ở đây)
                var khach = _context.KhachHangs.FirstOrDefault(k => k.MaTaiKhoan == taiKhoan.MaTaiKhoan);

                if (khach != null)
                {
                    // Lưu Session
                    HttpContext.Session.SetString("MaKhachHang", khach.MaKhachHang);
                    HttpContext.Session.SetString("TenDangNhap", taiKhoan.UserName ?? "");

                    // Lấy Tên & SĐT từ bảng KHACH_HANG by Hoang
                    HttpContext.Session.SetString("TenKhachHang", khach.TenKhachHang ?? "");
                    HttpContext.Session.SetString("SdtKhachHang", khach.SdtKhachHang ?? "");

                    // ✅ SỬA LẠI: Lấy Email từ bảng TAI_KHOAN (taiKhoan.Email) by Hoang
                    HttpContext.Session.SetString("EmailKhachHang", taiKhoan.Email ?? "");
                }

                TempData["Type"] = "success";
                TempData["Message"] = "Đăng nhập thành công!";
                return RedirectToAction("Index", "Home");
            }

            TempData["Type"] = "error";
            TempData["Message"] = "Sai tài khoản hoặc mật khẩu!";
            return View();
        }

        // --- 1. QUÊN MẬT KHẨU (Bước 1: Nhập Email) ---
        [HttpGet]
        public IActionResult QuenMatKhau()
        {
            return View();
        }

        [HttpPost]
        public IActionResult QuenMatKhau(string Email)
        {
            var taiKhoan = _context.TaiKhoans.FirstOrDefault(t => t.Email == Email);
            if (taiKhoan == null)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Email này không tồn tại.";
                return View();
            }

            string otp = new Random().Next(100000, 999999).ToString();

            HttpContext.Session.SetString("OTP_Reset", otp);
            HttpContext.Session.SetString("OTP_Email", Email);
            HttpContext.Session.SetString("OTP_Expiry", DateTime.Now.AddMinutes(10).ToString());

            try
            {
                // 3. GỌI SERVICE (Gọn gàng hơn hẳn!)
                string subject = "Mã OTP Đặt Lại Mật Khẩu";
                string body = $"<h3>Mã OTP của bạn là: <span style='color:red'>{otp}</span></h3><p>Có hiệu lực trong 10 phút.</p>";

                _emailService.GuiEmail(Email, subject, body);

                TempData["Type"] = "success";
                TempData["Title"] = "Đã gửi OTP";
                TempData["Message"] = $"Mã OTP đã được gửi đến <b>{Email}</b>";
                return RedirectToAction("XacThucOtp");
            }
            catch (Exception ex)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Lỗi gửi email: " + ex.Message;
                return View();
            }
        }

        // --- 2. XÁC THỰC OTP (Bước 2: Nhập Mã) ---
        [HttpGet]
        public IActionResult XacThucOtp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult XacThucOtp(string MaOtp)
        {
            var otpSession = HttpContext.Session.GetString("OTP_Reset");
            var expiryString = HttpContext.Session.GetString("OTP_Expiry");

            // Kiểm tra OTP
            if (string.IsNullOrEmpty(otpSession) || MaOtp != otpSession)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Mã OTP không đúng!";
                return View();
            }

            // Kiểm tra hết hạn
            if (DateTime.TryParse(expiryString, out DateTime expiry) && DateTime.Now > expiry)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Mã OTP đã hết hạn!";
                return RedirectToAction("QuenMatKhau");
            }

            // Đúng hết -> Cho phép đặt lại mật khẩu
            TempData["Type"] = "success";
            TempData["Message"] = "Xác thực thành công. Vui lòng đặt lại mật khẩu.";
            return RedirectToAction("DatLaiMatKhau");
        }

        // --- 3. ĐẶT LẠI MẬT KHẨU (Bước 3: Nhập MK Mới) ---
        [HttpGet]
        public IActionResult DatLaiMatKhau()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DatLaiMatKhau(string MatKhauMoi, string XacNhanMatKhauMoi)
        {
            if (MatKhauMoi != XacNhanMatKhauMoi)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Mật khẩu xác nhận không khớp!";
                return View();
            }

            var email = HttpContext.Session.GetString("OTP_Email");
            var taiKhoan = _context.TaiKhoans.FirstOrDefault(t => t.Email == email);

            if (taiKhoan != null)
            {
                taiKhoan.Password = MatKhauMoi; // Lưu mật khẩu mới
                _context.SaveChanges();

                // Xóa Session OTP
                HttpContext.Session.Remove("OTP_Reset");
                HttpContext.Session.Remove("OTP_Email");

                // Thông báo kèm mật khẩu mới (như bạn yêu cầu)
                TempData["Type"] = "success";
                TempData["Title"] = "Thành công";
                TempData["Message"] = $"Mật khẩu đã thay đổi.<br>Mật khẩu mới là: <b style='color:#cda45e'>{MatKhauMoi}</b>";

                return RedirectToAction("Index"); // Về trang đăng nhập
            }

            TempData["Type"] = "error";
            TempData["Message"] = "Có lỗi xảy ra, không tìm thấy tài khoản.";
            return RedirectToAction("Index");
        }

        // --- 4. ĐĂNG XUẤT (CHỈ GIỮ LẠI 1 HÀM DUY NHẤT) ---
        public IActionResult DangXuat()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // --- HELPER GỬI MAIL ---
        private void SendEmailOtp(string email, string otp)
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
                Subject = "Mã OTP Đặt Lại Mật Khẩu",
                Body = $"Mã OTP của bạn là: {otp}\nCó hiệu lực trong 10 phút.",
                IsBodyHtml = false,
            };
            mailMessage.To.Add(email);
            smtpClient.Send(mailMessage);
        }
    }
}
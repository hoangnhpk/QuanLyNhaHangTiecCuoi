using Microsoft.AspNetCore.Mvc;
using QuanLyNhaHang.Models;
using QuanLyNhaHang.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.Json;
using System.Collections.Generic; // Thêm namespace này

namespace QuanLyNhaHang.Controllers
{
    public class DangKyController : Controller
    {
        private readonly QuanLyNhaHangContext _context;

        public DangKyController(QuanLyNhaHangContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(KhachHangDangKyViewModel model)
        {
            // --- 1. KIỂM TRA TRÙNG LẶP DB (Thêm lỗi vào ModelState) ---

            // Kiểm tra trùng Tài khoản
            if (_context.TaiKhoans.Any(t => t.UserName == model.TaiKhoanKhachHang))
            {
                // Tham số đầu tiên phải trùng KHỚP với tên biến trong Model
                ModelState.AddModelError("TaiKhoanKhachHang", "Tài khoản này đã được sử dụng.");
            }

            // Kiểm tra trùng Email
            if (_context.TaiKhoans.Any(t => t.Email == model.EmailKhachHang))
            {
                ModelState.AddModelError("EmailKhachHang", "Email này đã tồn tại trong hệ thống.");
            }

            // Kiểm tra trùng SĐT
            if (_context.KhachHangs.Any(k => k.SdtKhachHang == model.SdtKhachHang))
            {
                ModelState.AddModelError("SdtKhachHang", "Số điện thoại này đã được đăng ký.");
            }

            // --- 2. KIỂM TRA TỔNG THỂ ---
            // ModelState.IsValid sẽ trả về false nếu:
            // - Vi phạm Validate trong Model (Regex, Required...)
            // - HOẶC vi phạm lỗi DB vừa add ở trên
            if (!ModelState.IsValid)
            {
                return View(model); // Trả về View để hiện lỗi span đỏ
            }

            // --- 3. NẾU KHÔNG CÓ LỖI -> GỬI OTP ---
            string otp = new Random().Next(100000, 999999).ToString();

            try
            {
                SendEmailOtp(model.EmailKhachHang, otp);
            }
            catch
            {
                ModelState.AddModelError("EmailKhachHang", "Không thể gửi OTP. Vui lòng kiểm tra lại email/mạng.");
                return View(model);
            }

            // Serialize và Lưu Session
            string modelJson = JsonSerializer.Serialize(model);
            HttpContext.Session.SetString("Pending_Register", modelJson);
            HttpContext.Session.SetString("OTP_Code", otp);
            HttpContext.Session.SetString("OTP_Expiry", DateTime.Now.AddMinutes(5).ToString());

            // Cấu hình Popup Thành Công
            TempData["Type"] = "success";
            TempData["Title"] = "Đã gửi mã OTP";
            TempData["Message"] = $"Mã xác thực đã được gửi tới <b>{model.EmailKhachHang}</b>";

            return RedirectToAction("XacThucOtp");
        }

        // ... (Giữ nguyên các hàm XacThucOtp và SendEmailOtp cũ)
        [HttpGet]
        public IActionResult XacThucOtp()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Pending_Register")))
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public IActionResult XacThucOtp(string MaOtp)
        {
            var otpSession = HttpContext.Session.GetString("OTP_Code");
            var expiryStr = HttpContext.Session.GetString("OTP_Expiry");
            var modelJson = HttpContext.Session.GetString("Pending_Register");

            if (string.IsNullOrEmpty(otpSession) || string.IsNullOrEmpty(modelJson))
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Phiên giao dịch hết hạn. Vui lòng đăng ký lại.";
                return RedirectToAction("Index");
            }

            if (MaOtp != otpSession)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Mã OTP không chính xác!";
                return View();
            }

            if (DateTime.TryParse(expiryStr, out DateTime expiry) && DateTime.Now > expiry)
            {
                TempData["Type"] = "error";
                TempData["Message"] = "Mã OTP đã hết hạn!";
                return RedirectToAction("Index");
            }

            var model = JsonSerializer.Deserialize<KhachHangDangKyViewModel>(modelJson);
            string maTK = "TK" + DateTime.Now.Ticks.ToString().Substring(10);
            string maKH = "KH" + DateTime.Now.Ticks.ToString().Substring(10);

            var taiKhoan = new TaiKhoan
            {
                MaTaiKhoan = maTK,
                UserName = model.TaiKhoanKhachHang,
                Password = model.MatKhauKhachHang,
                Email = model.EmailKhachHang,
                VaiTro = "Khách hàng",
                TrangThai = "Hoạt động"
            };

            var khachHang = new KhachHang
            {
                MaKhachHang = maKH,
                MaTaiKhoan = maTK,
                TenKhachHang = model.TenKhachHang,
                CccdKhachHang = model.CccdKhachHang,
                DiaChiKhachHang = model.DiaChiKhachHang,
                SdtKhachHang = model.SdtKhachHang,
                TrangThaiKhachHang = "Hoạt động"
            };

            _context.TaiKhoans.Add(taiKhoan);
            _context.KhachHangs.Add(khachHang);
            _context.SaveChanges();

            HttpContext.Session.Remove("Pending_Register");
            HttpContext.Session.Remove("OTP_Code");
            HttpContext.Session.Remove("OTP_Expiry");

            TempData["Type"] = "success";
            TempData["Title"] = "Thành công";
            TempData["Message"] = "Đăng ký tài khoản thành công! Mời bạn đăng nhập.";

            return RedirectToAction("Index", "DangNhap");
        }

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
                Subject = "Mã OTP Xác Thực Đăng Ký",
                Body = $"Mã OTP của bạn là: {otp}\nCó hiệu lực trong 5 phút.",
                IsBodyHtml = false,
            };
            mailMessage.To.Add(email);
            smtpClient.Send(mailMessage);
        }
    }
}
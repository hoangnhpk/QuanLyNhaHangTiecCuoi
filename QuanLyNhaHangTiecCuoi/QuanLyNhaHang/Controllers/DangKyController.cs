using Microsoft.AspNetCore.Mvc;
using QuanLyNhaHang.Models;
using QuanLyNhaHang.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.Json;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
            // --- 1. KIỂM TRA ĐỊNH DẠNG & TÊN MIỀN EMAIL ---
            if (!string.IsNullOrEmpty(model.EmailKhachHang) && !IsValidEmail(model.EmailKhachHang))
            {
                ModelState.AddModelError("EmailKhachHang", "Email không đúng định dạng hoặc tên miền không tồn tại.");
            }

            // --- 2. KIỂM TRA TRÙNG LẶP TRONG DATABASE ---

            // Kiểm tra trùng Tài khoản (Bảng TaiKhoans)
            if (_context.TaiKhoans.Any(t => t.UserName == model.TaiKhoanKhachHang))
            {
                ModelState.AddModelError("TaiKhoanKhachHang", "Tài khoản này đã được sử dụng.");
            }

            // Kiểm tra trùng Email (Bảng TaiKhoans)
            if (_context.TaiKhoans.Any(t => t.Email == model.EmailKhachHang))
            {
                ModelState.AddModelError("EmailKhachHang", "Email này đã tồn tại trong hệ thống.");
            }

            // Kiểm tra trùng Số điện thoại (Bảng KhachHangs)
            if (_context.KhachHangs.Any(k => k.SdtKhachHang == model.SdtKhachHang))
            {
                ModelState.AddModelError("SdtKhachHang", "Số điện thoại này đã được đăng ký.");
            }

            // Kiểm tra trùng CCCD/CMND (Bảng KhachHangs)
            if (_context.KhachHangs.Any(k => k.CccdKhachHang == model.CccdKhachHang))
            {
                ModelState.AddModelError("CccdKhachHang", "Số CCCD/CMND này đã được sử dụng.");
            }

            // --- 3. KIỂM TRA TỔNG THỂ ---
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // --- 4. NẾU KHÔNG CÓ LỖI -> GỬI OTP ---
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

            // Lưu dữ liệu tạm thời vào Session
            string modelJson = JsonSerializer.Serialize(model);
            HttpContext.Session.SetString("Pending_Register", modelJson);
            HttpContext.Session.SetString("OTP_Code", otp);
            HttpContext.Session.SetString("OTP_Expiry", DateTime.Now.AddMinutes(5).ToString());

            TempData["Type"] = "success";
            TempData["Title"] = "Đã gửi mã OTP";
            TempData["Message"] = $"Mã xác thực đã được gửi tới <b>{model.EmailKhachHang}</b>";

            return RedirectToAction("XacThucOtp");
        }

        // Hàm kiểm tra định dạng và sự tồn tại của tên miền Email
        private bool IsValidEmail(string email)
        {
            try
            {
                var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                if (!regex.IsMatch(email)) return false;

                var host = email.Split('@')[1];
                var hostEntry = Dns.GetHostAddresses(host);
                return hostEntry.Length > 0;
            }
            catch { return false; }
        }

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

            // Lưu chính thức vào Database
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

            // Xóa session sau khi xong
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
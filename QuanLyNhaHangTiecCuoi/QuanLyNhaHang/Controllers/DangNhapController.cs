using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyNhaHang.Models;
using QuanLyNhaHang.Services;
using QuanLyNhaHang.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QuanLyNhaHang.Controllers
{
    public class DangNhapController : Controller
    {
        private readonly QuanLyNhaHangContext _context;
        private readonly IEmailService _emailService;

        public DangNhapController(QuanLyNhaHangContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // ==========================================================
        // 1. ĐĂNG NHẬP HỆ THỐNG (GIỮ NGUYÊN)
        // ==========================================================
        [HttpGet]
        public IActionResult Index(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string TaiKhoan, string MatKhau, string returnUrl = null)
        {
            var user = _context.TaiKhoans.FirstOrDefault(t => (t.UserName == TaiKhoan || t.Email == TaiKhoan) && t.Password == MatKhau);

            if (user != null)
            {
                if (user.TrangThai != "Hoạt động")
                {
                    TempData["Type"] = "error";
                    TempData["Message"] = $"Tài khoản đang ở trạng thái: {user.TrangThai}. Vui lòng liên hệ Admin.";
                    return View();
                }

                string tenHienThi = user.UserName;
                string userId = user.MaTaiKhoan.ToString();
                string vaiTro = (user.VaiTro ?? "Khách hàng").Trim();

                if (vaiTro == "Admin" || vaiTro == "Quản lý")
                {
                    var qtv = _context.QuanTriViens.FirstOrDefault(q => q.MaTaiKhoan == user.MaTaiKhoan);
                    if (qtv != null) tenHienThi = qtv.TenQuanTriVien;
                }
                else
                {
                    var khach = _context.KhachHangs.FirstOrDefault(k => k.MaTaiKhoan == user.MaTaiKhoan);
                    if (khach != null)
                    {
                        tenHienThi = khach.TenKhachHang;
                        HttpContext.Session.SetString("MaKhachHang", khach.MaKhachHang);
                        HttpContext.Session.SetString("TenKhachHang", khach.TenKhachHang);
                        HttpContext.Session.SetString("SdtKhachHang", khach.SdtKhachHang ?? "");
                        HttpContext.Session.SetString("EmailKhachHang", user.Email ?? "");
                    }
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, tenHienThi),
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Role, MapRoleToCode(vaiTro)),
                    new Claim("Email", user.Email ?? "")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                TempData["Type"] = "success";
                if (vaiTro == "Admin") return RedirectToAction("Index", "QuanTriVien", new { area = "Admin" });
                if (vaiTro == "Quản lý") return RedirectToAction("Index", "QuanLyThucDon", new { area = "Admin" });

                return Redirect(Url.Action("Index", "Home") + "#book-a-table");
            }

            TempData["Type"] = "error";
            TempData["Message"] = "Sai tài khoản hoặc mật khẩu!";
            return View();
        }

        // ==========================================================
        // 2. QUÊN MẬT KHẨU / OTP (GIỮ NGUYÊN)
        // ==========================================================
        [HttpGet] public IActionResult QuenMatKhau() => View();

        [HttpPost]
        public IActionResult QuenMatKhau(string Email)
        {
            var taiKhoan = _context.TaiKhoans.FirstOrDefault(t => t.Email == Email);
            if (taiKhoan == null) { TempData["Message"] = "Email không tồn tại."; return View(); }

            string otp = new Random().Next(100000, 999999).ToString();
            HttpContext.Session.SetString("OTP_Reset", otp);
            HttpContext.Session.SetString("OTP_Email", Email);
            HttpContext.Session.SetString("OTP_Expiry", DateTime.Now.AddMinutes(10).ToString());

            _emailService.GuiEmail(Email, "Mã OTP", $"Mã OTP của bạn là: {otp}");
            return RedirectToAction("XacThucOtp");
        }

        [HttpGet] public IActionResult XacThucOtp() => View();
        [HttpPost]
        public IActionResult XacThucOtp(string MaOtp)
        {
            if (MaOtp == HttpContext.Session.GetString("OTP_Reset")) return RedirectToAction("DatLaiMatKhau");
            TempData["Message"] = "Mã OTP không đúng!";
            return View();
        }

        [HttpGet] public IActionResult DatLaiMatKhau() => View();
        [HttpPost]
        public IActionResult DatLaiMatKhau(string MatKhauMoi, string XacNhanMatKhauMoi)
        {
            if (MatKhauMoi != XacNhanMatKhauMoi) return View();
            var email = HttpContext.Session.GetString("OTP_Email");
            var tk = _context.TaiKhoans.FirstOrDefault(t => t.Email == email);
            if (tk != null) { tk.Password = MatKhauMoi; _context.SaveChanges(); }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DangXuat()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // ==========================================================
        // 3. LOGIC ĐĂNG NHẬP GOOGLE (PHẦN CẦN SỬA)
        // ==========================================================

        [HttpGet]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!result.Succeeded) return RedirectToAction("Index");

            var email = result.Principal.FindFirstValue(ClaimTypes.Email);
            var name = result.Principal.FindFirstValue(ClaimTypes.Name);

            // 1. Kiểm tra Email có tồn tại trong hệ thống chưa
            var user = _context.TaiKhoans.FirstOrDefault(t => t.Email == email);

            if (user != null)
            {
                // THỐNG NHẤT: NẾU ĐÃ CÓ TÀI KHOẢN -> ĐĂNG NHẬP THẲNG
                var khachInfo = _context.KhachHangs.FirstOrDefault(k => k.MaTaiKhoan == user.MaTaiKhoan);
                string tenHienThi = khachInfo?.TenKhachHang ?? name;

                return await SignInSystem(user, tenHienThi);
            }
            else
            {
                // TH2: CHƯA CÓ TRONG DB -> CHUYỂN ĐẾN TRANG NHẬP THÔNG TIN BỔ SUNG
                HttpContext.Session.SetString("GG_Email", email);
                HttpContext.Session.SetString("GG_Name", name);
                return RedirectToAction("HoanThienThongTin");
            }
        }

        [HttpGet]
        public IActionResult HoanThienThongTin()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("GG_Email")))
                return RedirectToAction("Index");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> HoanThienThongTin(HoanThienGoogleVM model)
        {
            var email = HttpContext.Session.GetString("GG_Email");
            var name = HttpContext.Session.GetString("GG_Name");

            if (string.IsNullOrEmpty(email)) return RedirectToAction("Index");

            // Kiểm tra trùng CCCD trong Database
            if (_context.KhachHangs.Any(k => k.CccdKhachHang == model.Cccd))
                ModelState.AddModelError("Cccd", "Số CCCD này đã tồn tại trên hệ thống.");

            if (!ModelState.IsValid) return View(model);

            // Tạo mã định danh mới
            string shortTicks = DateTime.Now.Ticks.ToString().Substring(10);
            string maTK = "TK" + shortTicks;
            string maKH = "KH" + shortTicks;

            var newUser = new TaiKhoan
            {
                MaTaiKhoan = maTK,
                UserName = email,
                Password = Guid.NewGuid().ToString().Substring(0, 8), // Mật khẩu ngẫu nhiên
                Email = email,
                VaiTro = "Khách hàng",
                TrangThai = "Hoạt động"
            };

            var newKhach = new KhachHang
            {
                MaKhachHang = maKH,
                MaTaiKhoan = maTK,
                TenKhachHang = name ?? "Khách hàng Google",
                CccdKhachHang = model.Cccd,
                SdtKhachHang = model.Sdt,
                DiaChiKhachHang = model.DiaChi,
                TrangThaiKhachHang = "Hoạt động"
            };

            _context.TaiKhoans.Add(newUser);
            _context.KhachHangs.Add(newKhach);
            await _context.SaveChangesAsync();

            // Xóa thông tin tạm sau khi lưu thành công
            HttpContext.Session.Remove("GG_Email");
            HttpContext.Session.Remove("GG_Name");

            return await SignInSystem(newUser, newKhach.TenKhachHang);
        }

        // ==========================================================
        // 4. HÀM DÙNG CHUNG (HELPER)
        // ==========================================================

        private async Task<IActionResult> SignInSystem(TaiKhoan user, string tenHienThi)
        {
            // Nạp dữ liệu vào Session cho các chức năng khác
            var khach = _context.KhachHangs.FirstOrDefault(k => k.MaTaiKhoan == user.MaTaiKhoan);
            if (khach != null)
            {
                HttpContext.Session.SetString("MaKhachHang", khach.MaKhachHang);
                HttpContext.Session.SetString("TenKhachHang", khach.TenKhachHang);
                HttpContext.Session.SetString("SdtKhachHang", khach.SdtKhachHang ?? "");
                HttpContext.Session.SetString("EmailKhachHang", user.Email ?? "");
            }

            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, tenHienThi),
                new Claim(ClaimTypes.NameIdentifier, user.MaTaiKhoan),
                new Claim(ClaimTypes.Role, MapRoleToCode(user.VaiTro)),
                new Claim("Email", user.Email ?? "")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            TempData["Type"] = "success";
            TempData["Message"] = $"Chào mừng <b>{tenHienThi}</b>!";

            // Phân quyền điều hướng
            if (user.VaiTro == "Admin") return RedirectToAction("Index", "QuanTriVien", new { area = "Admin" });
            if (user.VaiTro == "Quản lý") return RedirectToAction("Index", "QuanLyThucDon", new { area = "Admin" });

            return RedirectToAction("Index", "Home");
        }

        private string MapRoleToCode(string dbRole)
        {
            if (string.IsNullOrEmpty(dbRole)) return "KhachHang";
            dbRole = dbRole.Trim();
            if (dbRole == "Quản lý") return "QuanLy";
            if (dbRole == "Admin") return "Admin";
            return "KhachHang";
        }
    }
}
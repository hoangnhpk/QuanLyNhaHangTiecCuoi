using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
// --- THÊM THƯ VIỆN ĐỂ DÙNG COOKIE AUTH ---
using System.Security.Claims;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyNhaHang.Models;
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

        // --- TRANG ĐĂNG NHẬP ---
        [HttpGet]
        public IActionResult Index(string returnUrl = null)
        {
            // Nếu đã đăng nhập rồi thì không cho vào trang Login nữa -> Về trang chủ
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            // Lưu cái link cũ vào ViewData để lát nữa form POST gửi lên lại
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string TaiKhoan, string MatKhau, string returnUrl = null)
        {
            // 1. Tìm tài khoản
            var user = _context.TaiKhoans
                .FirstOrDefault(t => (t.UserName == TaiKhoan || t.Email == TaiKhoan) && t.Password == MatKhau);

            if (user != null)
            {
                // --- SỬA LẠI LOGIC CHECK TRẠNG THÁI (Dựa trên ảnh dữ liệu bạn gửi) ---
                // Chỉ cho phép đăng nhập nếu trạng thái là "Hoạt động"
                if (user.TrangThai != "Hoạt động")
                {
                    TempData["Type"] = "error";
                    // Hiển thị thông báo cụ thể (VD: Tài khoản đang bị Khóa)
                    TempData["Message"] = $"Tài khoản đang ở trạng thái: {user.TrangThai}. Vui lòng liên hệ Admin.";
                    return View();
                }

                // 2. Xử lý Vai Trò (Dữ liệu của bạn có dấu tiếng Việt)
                string tenHienThi = user.UserName;
                string userId = user.MaTaiKhoan.ToString();
                string vaiTro = user.VaiTro ?? "Khách hàng"; // Mặc định là Khách hàng

                // Chuẩn hóa chuỗi vai trò để so sánh (Trim để xóa khoảng trắng thừa nếu có)
                vaiTro = vaiTro.Trim();
                string maKhachHangHienTai = "";

                if (vaiTro == "Admin" || vaiTro == "Quản lý") // "Quản lý" có dấu
                {
                    var qtv = _context.QuanTriViens.FirstOrDefault(q => q.MaTaiKhoan == user.MaTaiKhoan);
                    if (qtv != null) tenHienThi = qtv.TenQuanTriVien;
                }
                else // "Khách hàng" Haong
                {
                    var khach = _context.KhachHangs.FirstOrDefault(k => k.MaTaiKhoan == user.MaTaiKhoan);
                    if (khach != null)
                    {
                        tenHienThi = khach.TenKhachHang;
                        maKhachHangHienTai = khach.MaKhachHang;

                        // --- BỔ SUNG: LƯU THÔNG TIN VÀO SESSION ---
                        HttpContext.Session.SetString("MaKhachHang", khach.MaKhachHang);
                        HttpContext.Session.SetString("TenKhachHang", khach.TenKhachHang);
                        HttpContext.Session.SetString("SdtKhachHang", khach.SdtKhachHang ?? "");
                        HttpContext.Session.SetString("EmailKhachHang", user.Email ?? "");
                        // -------------------------------------------
                    }
                }

                // 3. Tạo Claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, tenHienThi),
                    new Claim(ClaimTypes.NameIdentifier, userId),
                   new Claim("MaKH", maKhachHangHienTai),
                    new Claim(ClaimTypes.Role, MapRoleToCode(vaiTro)),
                    new Claim("Email", user.Email ?? "")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                // 4. Điều hướng
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                TempData["Type"] = "success";
                TempData["Message"] = "Đăng nhập thành công!";

                // Kiểm tra vai trò gốc từ DB để điều hướng
                // CASE 1: Nếu là Admin -> Trả về View Quản Trị Viên
                // Tương ứng với thư mục View: QuanTriVien
                if (vaiTro == "Admin")
                {
                    // Thông báo trong Popup
                    TempData["Message"] = "Xin chào Administrator. Chúc bạn một ngày làm việc hiệu quả!";
                    // Thông báo khung xanh (Inline Alert) trong View Admin
                    TempData["SuccessMessage"] = "Chào mừng Admin quay trở lại hệ thống!";

                    return RedirectToAction("Index", "QuanTriVien", new { area = "Admin" });
                }
                else if (vaiTro == "Quản lý")
                {
                    // Thông báo trong Popup
                    TempData["Message"] = $"Xin chào Quản lý {tenHienThi}.";
                    // Thông báo khung xanh (Inline Alert) trong View Quản lý
                    TempData["SuccessMessage"] = "Đăng nhập thành công. Hãy kiểm tra thực đơn hôm nay!";

                    return RedirectToAction("Index", "QuanLyThucDon", new { area = "Admin" });
                }
                else // Khách hàng
                {
                    // Khách hàng chỉ cần Popup, không cần khung xanh inline
                    TempData["Message"] = $"Chào mừng <b>{tenHienThi}</b> đến với nhà hàng Long Phụng!";

                    // Lưu ý: Khách hàng return về Home
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return Redirect(Url.Action("Index", "Home") + "#book-a-table");
                }
            }

            ViewData["ReturnUrl"] = returnUrl;
            TempData["Type"] = "error";
            TempData["Message"] = "Sai tài khoản hoặc mật khẩu!";
            return View();
        }

        // --- HÀM PHỤ: CHUYỂN ĐỔI VAI TRÒ TỪ TIẾNG VIỆT SANG MÃ CODE ---
        // Giúp bạn dùng [Authorize(Roles="QuanLy")] thay vì phải gõ tiếng Việt trong code
        private string MapRoleToCode(string dbRole)
        {
            if (dbRole == "Quản lý") return "QuanLy";
            if (dbRole == "Khách hàng") return "KhachHang";
            if (dbRole == "Admin") return "Admin";
            return "Guest";
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
        public async Task<IActionResult> DangXuat()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
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
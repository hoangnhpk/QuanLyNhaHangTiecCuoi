using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using QuanLyNhaHang.Models;
using QuanLyNhaHang.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyNhaHang.Controllers
{
    public class TTCaNhanController : Controller
    {
        private readonly QuanLyNhaHangContext _context;

        public TTCaNhanController(QuanLyNhaHangContext context)
        {
            _context = context;
        }

        // GET: /TTCaNhan
        public async Task<IActionResult> Index()
        {
            try
            {
                // Lấy mã khách hàng từ session/claim
                var maKhachHang = User.FindFirst("MaKhachHang")?.Value
                                 ?? HttpContext.Session.GetString("MaKhachHang")
                                 ?? "KH001"; // Tạm thời

                // LUÔN LẤY THÔNG TIN TỪ DATABASE
                var khachHang = await _context.KhachHangs
                    .Include(k => k.TaiKhoan)
                    .AsNoTracking() // Chỉ đọc, không theo dõi thay đổi
                    .FirstOrDefaultAsync(k => k.MaKhachHang == maKhachHang);

                if (khachHang == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy thông tin khách hàng.";
                    return RedirectToAction("Login", "Account");
                }

                // LUÔN LẤY SDT TỪ DATABASE
                var sdtFromDb = khachHang.SdtKhachHang;

                // LUÔN LẤY EMAIL TỪ DATABASE
                var emailFromDb = khachHang.TaiKhoan?.Email ?? "Chưa có email";

                // Truyền thông tin qua ViewBag
                ViewBag.Email = emailFromDb;
                ViewBag.Sdt = sdtFromDb;
                ViewBag.Message = TempData["Message"];

                return View(khachHang);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Lỗi hệ thống: " + ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: /TTCaNhan/GetEditModel
        [HttpGet]
        public async Task<IActionResult> GetEditModel(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(new { success = false, message = "Mã khách hàng không hợp lệ" });
            }

            // LUÔN LẤY THÔNG TIN CŨ TỪ DATABASE
            var khachHang = await _context.KhachHangs
                .Include(k => k.TaiKhoan)
                .FirstOrDefaultAsync(k => k.MaKhachHang == id);

            if (khachHang == null)
            {
                return Json(new { success = false, message = "Không tìm thấy khách hàng" });
            }

            // Tạo ViewModel từ thông tin database
            var viewModel = new KhachHangEditViewModel
            {
                MaKhachHang = khachHang.MaKhachHang,
                MaTaiKhoan = khachHang.MaTaiKhoan,
                TenKhachHang = khachHang.TenKhachHang,
                CccdKhachHang = khachHang.CccdKhachHang,
                SdtKhachHang = khachHang.SdtKhachHang, // Lấy từ database
                DiaChiKhachHang = khachHang.DiaChiKhachHang,
                TrangThaiKhachHang = khachHang.TrangThaiKhachHang,
                GhiChu = khachHang.GhiChu,
                Email = khachHang.TaiKhoan?.Email // Lấy từ database
            };

            return Json(new { success = true, data = viewModel });
        }

        // POST: /TTCaNhan/Edit (AJAX)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(KhachHangEditViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return Json(new { success = false, message = string.Join(", ", errors) });
                }

                // LUÔN LẤY THÔNG TIN CŨ TỪ DATABASE TRƯỚC KHI CẬP NHẬT
                var existingKhachHang = await _context.KhachHangs
                    .FirstOrDefaultAsync(k => k.MaKhachHang == model.MaKhachHang);

                if (existingKhachHang == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy khách hàng" });
                }

                // Cập nhật các trường được phép
                existingKhachHang.TenKhachHang = model.TenKhachHang;
                existingKhachHang.CccdKhachHang = model.CccdKhachHang;
                existingKhachHang.DiaChiKhachHang = model.DiaChiKhachHang;
                existingKhachHang.TrangThaiKhachHang = model.TrangThaiKhachHang;
                existingKhachHang.GhiChu = model.GhiChu;

                // KHÔNG CẬP NHẬT SĐT - LUÔN GIỮ SĐT CŨ TỪ DATABASE
                // KHÔNG CẬP NHẬT EMAIL - EMAIL ĐƯỢC LƯU TRONG BẢNG TAIKHOAN

                await _context.SaveChangesAsync();

                TempData["Message"] = "Cập nhật thông tin thành công!";
                return Json(new
                {
                    success = true,
                    message = "Cập nhật thành công",
                    sdt = existingKhachHang.SdtKhachHang // Trả về SĐT cũ từ database
                });
            }
            catch (DbUpdateException ex)
            {
                return Json(new { success = false, message = "Lỗi cơ sở dữ liệu: " + ex.InnerException?.Message ?? ex.Message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        // GET: /TTCaNhan/GetCurrentInfo
        [HttpGet]
        public async Task<IActionResult> GetCurrentInfo()
        {
            var maKhachHang = User.FindFirst("MaKhachHang")?.Value
                             ?? HttpContext.Session.GetString("MaKhachHang")
                             ?? "KH001";

            var khachHang = await _context.KhachHangs
                .Include(k => k.TaiKhoan)
                .FirstOrDefaultAsync(k => k.MaKhachHang == maKhachHang);

            if (khachHang == null)
            {
                return Json(new { success = false, message = "Không tìm thấy thông tin" });
            }

            return Json(new
            {
                success = true,
                sdt = khachHang.SdtKhachHang,
                email = khachHang.TaiKhoan?.Email,
                ten = khachHang.TenKhachHang
            });
        }
    }
}
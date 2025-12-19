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

        /// <summary>
        /// Hàm hỗ trợ lấy Mã Khách Hàng từ Session để đảm bảo tính động và bảo mật
        /// </summary>
        private string GetCurrentMaKhachHang()
        {
            return HttpContext.Session.GetString("MaKhachHang");
        }

        // GET: /TTCaNhan
        public async Task<IActionResult> Index()
        {
            try
            {
                // 1. Lấy ID từ Session của người đang đăng nhập
                var maKhachHang = GetCurrentMaKhachHang();

                // 2. Nếu Session trống (hết hạn hoặc chưa đăng nhập), chuyển về trang đăng nhập
                if (string.IsNullOrEmpty(maKhachHang))
                {
                    TempData["Type"] = "error";
                    TempData["Message"] = "Phiên làm việc đã hết hạn. Vui lòng đăng nhập lại.";
                    return RedirectToAction("Index", "DangNhap");
                }

                // 3. Truy vấn dữ liệu mới nhất từ Database theo ID động
                var khachHang = await _context.KhachHangs
                    .Include(k => k.TaiKhoan)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(k => k.MaKhachHang == maKhachHang);

                if (khachHang == null)
                {
                    return RedirectToAction("Index", "DangNhap");
                }

                // 4. Chuẩn bị dữ liệu hiển thị
                ViewBag.Email = khachHang.TaiKhoan?.Email ?? "Chưa có email";
                ViewBag.Sdt = khachHang.SdtKhachHang;
                ViewBag.Message = TempData["Message"];

                return View(khachHang);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Lỗi hệ thống: " + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: /TTCaNhan/GetEditModel (Sử dụng cho AJAX Load Modal)
        [HttpGet]
        public async Task<IActionResult> GetEditModel()
        {
            // TỐI ƯU: Không nhận 'id' từ Client để tránh việc thay đổi ID qua F12
            var maKhachHang = GetCurrentMaKhachHang();

            if (string.IsNullOrEmpty(maKhachHang))
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập lại!" });
            }

            var khachHang = await _context.KhachHangs
                .Include(k => k.TaiKhoan)
                .FirstOrDefaultAsync(k => k.MaKhachHang == maKhachHang);

            if (khachHang == null)
            {
                return Json(new { success = false, message = "Không tìm thấy dữ liệu khách hàng." });
            }

            // Chuyển đổi sang ViewModel để gửi về Client
            var viewModel = new KhachHangEditViewModel
            {
                MaKhachHang = khachHang.MaKhachHang,
                TenKhachHang = khachHang.TenKhachHang,
                CccdKhachHang = khachHang.CccdKhachHang,
                SdtKhachHang = khachHang.SdtKhachHang,
                DiaChiKhachHang = khachHang.DiaChiKhachHang,
                GhiChu = khachHang.GhiChu,
                Email = khachHang.TaiKhoan?.Email
            };

            return Json(new { success = true, data = viewModel });
        }

        // POST: /TTCaNhan/Edit (Xử lý lưu dữ liệu qua AJAX)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(KhachHangEditViewModel model)
        {
            try
            {
                // BẢO MẬT: Kiểm tra ID trong Form gửi lên có khớp với ID đang đăng nhập không
                var currentMaKH = GetCurrentMaKhachHang();
                if (model.MaKhachHang != currentMaKH)
                {
                    return Json(new { success = false, message = "Thao tác không hợp lệ!" });
                }

                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Dữ liệu nhập vào chưa đúng định dạng." });
                }

                var existingKhachHang = await _context.KhachHangs
                    .FirstOrDefaultAsync(k => k.MaKhachHang == model.MaKhachHang);

                if (existingKhachHang == null)
                {
                    return Json(new { success = false, message = "Khách hàng không tồn tại." });
                }

                // Cập nhật các thông tin được phép sửa
                existingKhachHang.TenKhachHang = model.TenKhachHang;
                existingKhachHang.DiaChiKhachHang = model.DiaChiKhachHang;
                existingKhachHang.GhiChu = model.GhiChu;
                // CCCD, SĐT và Email được giữ nguyên theo logic bảo mật của bạn

                await _context.SaveChangesAsync();

                // Cập nhật lại tên trong Session để Header/Menu hiển thị tên mới ngay lập tức
                HttpContext.Session.SetString("TenKhachHang", model.TenKhachHang);

                return Json(new { success = true, message = "Thông tin cá nhân đã được cập nhật!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi khi lưu dữ liệu: " + ex.Message });
            }
        }
    }
}
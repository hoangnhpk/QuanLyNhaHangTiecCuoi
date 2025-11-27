using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyNhaHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyNhaHang.Controllers
{
    [Authorize(Roles = "QuanLy")]
    public class QL_KhachHangController : Controller
    {
        private readonly QuanLyNhaHangContext _context;

        public QL_KhachHangController(QuanLyNhaHangContext context)
        {
            _context = context;
        }

        // GET: QL_KhachHang
        public async Task<IActionResult> Index()
        {
            var khachHangs = await _context.KhachHangs
                .Include(k => k.TaiKhoan)
                .ToListAsync();
            return View(khachHangs);
        }

        // Phương thức giả định hash mật khẩu
        private string HashPassword(string password)
        {
            // ⚠️ CẦN THAY THẾ bằng thuật toán mã hóa mật khẩu an toàn (như BCrypt, PBKDF2).
            // Ví dụ: return BCrypt.Net.BCrypt.HashPassword(password);
            return $"HASHED_{password}"; // 👈 Đây chỉ là giá trị giả định
        }

        // ----------------------------------------------------
        // GET: QL_KhachHang/Details/5 (BỔ SUNG)
        // ----------------------------------------------------
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs
                .Include(k => k.TaiKhoan) // Cần include để lấy thông tin tài khoản
                .FirstOrDefaultAsync(m => m.MaKhachHang == id);

            if (khachHang == null)
            {
                return NotFound();
            }

            return View(khachHang);
        }

        // ----------------------------------------------------
        // GET: QL_KhachHang/Create (GIỮ NGUYÊN)
        // ----------------------------------------------------
        public IActionResult Create()
        {
            ViewData["MaTaiKhoan"] = new SelectList(_context.TaiKhoans, "MaTaiKhoan", "MaTaiKhoan");
            return View();
        }

        // ----------------------------------------------------
        // POST: QL_KhachHang/Create (GIỮ NGUYÊN)
        // ----------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaKhachHang,TenKhachHang,CccdKhachHang,SdtKhachHang,DiaChiKhachHang,TrangThaiKhachHang,GhiChu,MaTaiKhoan")] KhachHang khachHang)
        {
            // Loại bỏ validation cho DatTiecs và TaiKhoan
            ModelState.Remove("DatTiecs");
            ModelState.Remove("TaiKhoan");

            // Xử lý NULL values trước khi lưu
            khachHang.TenKhachHang ??= string.Empty;
            khachHang.CccdKhachHang ??= string.Empty;
            khachHang.SdtKhachHang ??= string.Empty;
            khachHang.DiaChiKhachHang ??= string.Empty;
            khachHang.TrangThaiKhachHang ??= "Active";
            khachHang.GhiChu ??= string.Empty;
            khachHang.MaTaiKhoan ??= null;

            // Vùng 1: KIỂM TRA TRÙNG MÃ KHI THÊM MỚI
            if (await KhachHangExistsAsync(khachHang.MaKhachHang))
            {
                ModelState.AddModelError("MaKhachHang", $"Mã khách hàng '{khachHang.MaKhachHang}' đã tồn tại. Vui lòng nhập mã khác.");
            }

            if (ModelState.IsValid)
            {
                // ... (Logic lưu Create) ...
                _context.Add(khachHang);
                await _context.SaveChangesAsync();

                // 🔔 THÊM THÔNG BÁO THÀNH CÔNG
                TempData["SuccessMessage"] = "Thêm Khách hàng mới thành công!";

                return RedirectToAction(nameof(Index));
            }

            // 💡 Cung cấp lại danh sách MaTaiKhoan nếu ModelState không hợp lệ
            ViewData["MaTaiKhoan"] = new SelectList(_context.TaiKhoans, "MaTaiKhoan", "MaTaiKhoan", khachHang.MaTaiKhoan);
            return View(khachHang);
        }

        // ----------------------------------------------------
        // GET: QL_KhachHang/Edit/5 (GIỮ NGUYÊN)
        // ----------------------------------------------------
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Cần Include TaiKhoan để hiển thị thông tin tài khoản trong View
            var khachHang = await _context.KhachHangs
                                         .Include(kh => kh.TaiKhoan)
                                         .FirstOrDefaultAsync(m => m.MaKhachHang == id);

            if (khachHang == null)
            {
                return NotFound();
            }

            // 💡 Quan trọng: Đảm bảo trường Mật Khẩu (Password) trong View trống
            if (khachHang.TaiKhoan != null)
            {
                khachHang.TaiKhoan.Password = null;
            }

            return View(khachHang);
        }

        // ----------------------------------------------------
        // POST: QL_KhachHang/Edit/5 (GIỮ NGUYÊN)
        // ----------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,
            [Bind("MaKhachHang,TenKhachHang,CccdKhachHang,SdtKhachHang,DiaChiKhachHang,TrangThaiKhachHang,GhiChu,MaTaiKhoan,TaiKhoan")] KhachHang khachHang)
        {
            if (id != khachHang.MaKhachHang)
            {
                return NotFound();
            }

            var originalKhachHang = await _context.KhachHangs
                .Include(kh => kh.TaiKhoan)
                .FirstOrDefaultAsync(m => m.MaKhachHang == id);

            if (originalKhachHang == null)
            {
                return NotFound();
            }

            ModelState.Remove("DatTiecs");
            ModelState.Remove("TaiKhoan");

            if (ModelState.IsValid)
            {
                try
                {
                    originalKhachHang.TenKhachHang = khachHang.TenKhachHang ?? string.Empty;
                    originalKhachHang.CccdKhachHang = khachHang.CccdKhachHang ?? string.Empty;
                    originalKhachHang.SdtKhachHang = khachHang.SdtKhachHang ?? string.Empty;
                    originalKhachHang.DiaChiKhachHang = khachHang.DiaChiKhachHang ?? string.Empty;
                    originalKhachHang.TrangThaiKhachHang = khachHang.TrangThaiKhachHang ?? "Active";
                    originalKhachHang.GhiChu = khachHang.GhiChu ?? string.Empty;

                    if (originalKhachHang.TaiKhoan != null && khachHang.TaiKhoan != null)
                    {
                        var newPasswordFromForm = khachHang.TaiKhoan.Password;

                        if (!string.IsNullOrEmpty(newPasswordFromForm))
                        {
                            originalKhachHang.TaiKhoan.Password = HashPassword(newPasswordFromForm);
                        }

                        originalKhachHang.TaiKhoan.Email = khachHang.TaiKhoan.Email ?? string.Empty;
                        originalKhachHang.TaiKhoan.UserName = khachHang.TaiKhoan.UserName ?? string.Empty;
                        originalKhachHang.TaiKhoan.TrangThai = khachHang.TaiKhoan.TrangThai ?? "Active";
                    }

                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = $"Cập nhật Khách hàng '{khachHang.MaKhachHang}' thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KhachHangExists(khachHang.MaKhachHang))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(khachHang);
        }

        // ----------------------------------------------------
        // GET: QL_KhachHang/Delete/5 (BỔ SUNG)
        // ----------------------------------------------------
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs
                .Include(k => k.TaiKhoan)
                .FirstOrDefaultAsync(m => m.MaKhachHang == id);

            if (khachHang == null)
            {
                return NotFound();
            }

            return View(khachHang);
        }

        // ----------------------------------------------------
        // POST: QL_KhachHang/Delete/5 (BỔ SUNG)
        // ----------------------------------------------------
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var khachHang = await _context.KhachHangs
                .Include(k => k.TaiKhoan)
                .FirstOrDefaultAsync(m => m.MaKhachHang == id);

            if (khachHang != null)
            {
                try
                {
                    // Xóa Tài khoản liên quan trước (nếu không có Cascade Delete được cấu hình)
                    if (khachHang.TaiKhoan != null)
                    {
                        _context.TaiKhoans.Remove(khachHang.TaiKhoan);
                    }

                    _context.KhachHangs.Remove(khachHang);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = $"Đã **xóa** khách hàng '{khachHang.TenKhachHang}' (Mã: {id}) thành công.";
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi nếu có ràng buộc khóa ngoại (ví dụ: khách hàng này đã có DatTiec)
                    TempData["ErrorMessage"] = $"Lỗi xóa khách hàng '{id}'. Khách hàng có thể đang liên kết với các dữ liệu khác.";
                    // Log lỗi (ex)
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // ----------------------------------------------------
        // POST: QL_KhachHang/Lock/5 (GIỮ NGUYÊN)
        // ----------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Lock(string id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy mã khách hàng để khóa.";
                return RedirectToAction(nameof(Index));
            }

            var khachHang = await _context.KhachHangs
                .Include(k => k.TaiKhoan)
                .FirstOrDefaultAsync(k => k.MaKhachHang == id);

            if (khachHang == null)
            {
                TempData["ErrorMessage"] = "Khách hàng không tồn tại.";
                return RedirectToAction(nameof(Index));
            }

            // Kiểm tra xem khách hàng đã bị khóa chưa để tránh thao tác dư thừa
            if (khachHang.TrangThaiKhachHang == "Inactive")
            {
                TempData["ErrorMessage"] = $"Khách hàng '{khachHang.TenKhachHang}' đã bị khóa trước đó.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                // Khóa thông tin Khách hàng
                khachHang.TrangThaiKhachHang = "Inactive";

                // Khóa Tài khoản đăng nhập (nếu có)
                if (khachHang.TaiKhoan != null)
                {
                    khachHang.TaiKhoan.TrangThai = "Inactive";
                }

                // Cập nhật trạng thái
                _context.Update(khachHang);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Đã **khóa** khách hàng '{khachHang.TenKhachHang}' (Mã: {khachHang.MaKhachHang}) thành công.";
            }
            catch (DbUpdateConcurrencyException)
            {
                TempData["ErrorMessage"] = "Lỗi đồng thời khi khóa tài khoản.";
            }


            return RedirectToAction(nameof(Index));
        }


        // ----------------------------------------------------
        // Phương thức kiểm tra tồn tại (GIỮ NGUYÊN)
        // ----------------------------------------------------
        private bool KhachHangExists(string id)
        {
            return _context.KhachHangs.Any(e => e.MaKhachHang == id);
        }

        private async Task<bool> KhachHangExistsAsync(string id)
        {
            return await _context.KhachHangs.AnyAsync(e => e.MaKhachHang == id);
        }
    }
}